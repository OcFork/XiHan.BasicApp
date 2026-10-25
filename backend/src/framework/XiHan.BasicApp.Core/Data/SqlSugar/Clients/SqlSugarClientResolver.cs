// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Options;
using SqlSugar;
using XiHan.BasicApp.Core.Data.SqlSugar.Options;
using XiHan.BasicApp.Core.Data.SqlSugar.Routing;
using XiHan.BasicApp.Core.Data.SqlSugar.Tenanting;
using XiHan.BasicApp.Core.Data;
using XiHan.BasicApp.Core.Exceptions;
using XiHan.BasicApp.Core.Security;
using XiHan.BasicApp.Core.Tenancy;
using XiHan.Framework.Kernel.Pipeline;
using XiHan.Framework.Kernel.Security;
using XiHan.Framework.MultiTenancy.Abstractions.Sharding;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Core.Data;

/// <summary>
/// SqlSugar 客户端解析器默认实现
/// </summary>
/// <remarks>
/// 基于 <see cref="SqlSugarScope"/>（线程安全单例）+ <see cref="ISqlSugarTenantConnectionResolver"/> 组合。
/// 当前租户上下文变化时，由 <c>ISqlSugarTenantConnectionResolver</c> 重新解析 ConfigId。
/// <para>
/// <b>事务钉连接</b>：<see cref="SqlSugarScope"/> 按异步上下文惰性创建客户端，且 AsyncLocal 在 async 方法返回后不回流调用方——
/// 若事务型工作单元期间持有 <see cref="SqlSugarScopeProvider"/>（其每次 <c>.Ado</c> 都重新解析当前上下文），
/// 同一工作单元内后续仓储调用可能落在无事务的新上下文连接上自动提交，而提交帧解析到的裸 provider 对空事务静默 no-op，
/// 造成首个写入永不提交（静默丢写）。因此事务型工作单元首次触达某 ConfigId 时，立即物化当前帧的
/// <see cref="SqlSugarScopeProvider.ScopedContext"/>（具体 <see cref="SqlSugarProvider"/>）钉入 <c>IUnitOfWork.Items</c>，
/// 同一工作单元内的所有后续解析直接复用，保证全部操作与 Begin/Commit/Rollback 落在同一连接、同一事务上。
/// 工作单元内的数据访问是顺序语义（并行访问共享连接本就不受 SqlSugar 支持），Items 用普通字典无并发问题。
/// </para>
/// </remarks>
public sealed class SqlSugarClientResolver : ISqlSugarClientResolver
{

    private readonly SqlSugarScope _sqlSugarScope;
    private readonly ISqlSugarTenantConnectionResolver _tenantConnectionResolver;
    private readonly IEntityModuleDataSourceResolver _entityModuleDataSourceResolver;
    private readonly IModuleDataSourceConnectionResolver _moduleDataSourceConnectionResolver;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISqlSugarConnectionConfigurator _connectionConfigurator;
    private readonly IOptions<XiHanSqlSugarCoreOptions> _options;
    private readonly IInvocationAccessor _invocationAccessor;
    private readonly ITenantShardResolver? _shardResolver;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sqlSugarScope">SqlSugar 根作用域</param>
    /// <param name="tenantConnectionResolver">租户连接解析器</param>
    /// <param name="entityModuleDataSourceResolver">实体模块数据源解析器</param>
    /// <param name="moduleDataSourceConnectionResolver">模块数据源连接解析器</param>
    /// <param name="currentTenant">当前租户</param>
    /// <param name="connectionConfigurator">连接配置器</param>
    /// <param name="options">SqlSugarCore 选项（租户连接的 ConfigId 前缀）</param>
    /// <param name="invocationAccessor">当前调用访问器：Tenant 阶段解析好的分片在调用上下文里</param>
    /// <param name="shardResolvers">5.0 租户分片解析器（可选，业务层实现库隔离时注册，取最后一个登记；未注册则退化为静态 ConfigId 解析）</param>
    public SqlSugarClientResolver(
        SqlSugarScope sqlSugarScope,
        ISqlSugarTenantConnectionResolver tenantConnectionResolver,
        IEntityModuleDataSourceResolver entityModuleDataSourceResolver,
        IModuleDataSourceConnectionResolver moduleDataSourceConnectionResolver,
        ICurrentTenant currentTenant,
        ISqlSugarConnectionConfigurator connectionConfigurator,
        IOptions<XiHanSqlSugarCoreOptions> options,
        IInvocationAccessor invocationAccessor,
        IEnumerable<ITenantShardResolver> shardResolvers)
    {
        _sqlSugarScope = sqlSugarScope;
        _tenantConnectionResolver = tenantConnectionResolver;
        _entityModuleDataSourceResolver = entityModuleDataSourceResolver;
        _moduleDataSourceConnectionResolver = moduleDataSourceConnectionResolver;
        _currentTenant = currentTenant;
        _connectionConfigurator = connectionConfigurator;
        _options = options;
        _invocationAccessor = invocationAccessor;
        // 与内核中间件的 GetService 一致：多个登记时最后一个生效（应用的解析器顶替画像默认的）
        _shardResolver = shardResolvers.LastOrDefault();
    }

    /// <summary>
    /// 获取当前租户对应的客户端，租户声明了独立连接则解析其独立连接，否则按 ConfigId 解析
    /// </summary>
    /// <returns>当前 Scope 级客户端</returns>
    /// <inheritdoc />
    public string? CurrentTenantId => _currentTenant.Id;

    public ISqlSugarClient GetCurrentClient()
    {
        return GetClient(ResolveCurrentLayoutConfigId());
    }

    /// <summary>
    /// 解析当前租户所在的那套数据库布局的父连接 ConfigId
    /// </summary>
    /// <remarks>
    /// 「布局」= 一个主库加上它下面的若干模块库。租户维度只决定用哪一套布局；
    /// 模块维度在布局内部选库。两者分别住在父 ConfigId 与模块名里，结构上不相交。
    /// 库隔离租户的连接由提供器给出并在此按需建连（描述符自带的模块库一并注册）。
    /// </remarks>
    /// <returns>父连接 ConfigId</returns>
    private string ResolveCurrentLayoutConfigId()
    {
        return ResolveCurrentLayout().ParentConfigId;
    }

    /// <summary>
    /// 解析当前租户所在的那套数据库布局
    /// </summary>
    /// <returns>父连接 ConfigId 与该租户自带的模块库配置（无则为 null）</returns>
    private (string ParentConfigId, List<SqlSugarModuleDataSourceConfigOptions>? ModuleConfigs) ResolveCurrentLayout()
    {
        // 库隔离：装配了分片解析器且处于业务租户上下文时，优先解析该租户的独立布局（平台就是 0 号租户，走默认布局）
        // 无分片 → 走静态 ConfigId 解析（字段/行隔离）；解析失败或抛异常 → fail-closed
        if (_shardResolver is not null && _currentTenant.Id is { } tenantId && !TenantIds.IsPlatform(tenantId)
            && ResolveShard(tenantId).TryGetValue(out var shard))
        {
            var descriptor = TenantShardConnections.ToConnection(shard, _options.Value);
            _ = _connectionConfigurator.EnsureTenantConnection(_sqlSugarScope, descriptor);
            return (descriptor.ConfigId.Trim(), descriptor.ModuleDataSourceConfigs);
        }

        return (_tenantConnectionResolver.ResolveCurrentConfigId(), null);
    }

    /// <summary>
    /// 取租户的分片：当前调用就是这个租户时直接读 Tenant 阶段写进上下文的分片（不触存储）；
    /// 否则（ICurrentTenant.Change 切到了别的租户，或不在调用管道里）向解析器要，失败按 fail-closed 抛出
    /// </summary>
    /// <param name="tenantId">租户标识</param>
    /// <returns>分片；无需独立连接时为无值</returns>
    private Maybe<TenantShard> ResolveShard(string tenantId)
    {
        if (_invocationAccessor.Current.TryGetValue(out var context)
            && TenantIds.AreSame(context.GetSecurity().Tenant?.Id, tenantId))
        {
            return context.GetTenantShard();
        }

        // 解析器按租户缓存，这里的同步等待只在切换租户的少数路径上发生
        return _shardResolver!.ResolveAsync(tenantId, CancellationToken.None).AsTask().GetAwaiter().GetResult().ValueOrThrow();
    }

    /// <summary>
    /// 获取实体对应的客户端：固定落平台库的取平台库；未声明模块数据源取当前租户主库，声明了则取该布局下的模块库
    /// </summary>
    /// <remarks>
    /// 两条维度在此交汇：租户维度先定「哪一套布局」，模块维度再在布局内选库。
    /// 模块数据源在当前布局与默认布局里都没有配置时 fail-closed 抛异常，
    /// 避免静默落到主库造成跨库串写。
    /// </remarks>
    /// <param name="entityType">实体类型</param>
    /// <returns>Scope 级客户端</returns>
    public ISqlSugarClient GetClientForEntity(Type entityType)
    {
        ArgumentNullException.ThrowIfNull(entityType);

        // 固定落平台库的实体（平台目录、账号、成员关系、读共享模板）：不随当前租户的独立布局切换，
        // 行级租户过滤照常生效；字段隔离的租户本就在平台库，这里对它们是同一个连接
        if (_entityModuleDataSourceResolver.IsPlatformPlaced(entityType))
        {
            return GetClient(_tenantConnectionResolver.ResolveConfigId(null));
        }

        var parentConfigId = ResolveCurrentLayoutConfigId();

        var moduleDataSource = _entityModuleDataSourceResolver.ResolveModuleDataSource(entityType);
        if (string.IsNullOrWhiteSpace(moduleDataSource))
        {
            return GetClient(parentConfigId);
        }

        var client = _moduleDataSourceConnectionResolver.ResolveClient(moduleDataSource, parentConfigId);
        return EnlistCurrentUnitOfWork(client);
    }

    /// <summary>
    /// 按 ConfigId 获取指定客户端，并登记进当前事务型工作单元
    /// </summary>
    /// <param name="configId">连接配置标识</param>
    /// <returns>Scope 级客户端</returns>
    public ISqlSugarClient GetClient(string configId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(configId);
        var client = _sqlSugarScope.GetConnectionScope(configId.Trim());
        return EnlistCurrentUnitOfWork(client);
    }

    /// <summary>
    /// 获取全部连接配置标识
    /// </summary>
    /// <returns>连接配置标识集合</returns>
    public IReadOnlyCollection<string> GetAllConfigIds()
    {
        return _tenantConnectionResolver.GetConfigIds();
    }

    /// <summary>
    /// 获取当前租户所在布局的全部连接配置标识：主库在前，其下已建连的模块库在后
    /// </summary>
    /// <remarks>
    /// 给「只初始化这一个租户」的场景用（如库隔离租户开通时建库建表）：
    /// <see cref="GetAllConfigIds"/> 给的是静态配置里的全量库，覆盖不到运行时才建连的租户库；
    /// 本方法给的是当前上下文这一套布局，租户自带的模块库也在内。
    /// </remarks>
    /// <returns>连接配置标识集合</returns>
    public IReadOnlyList<string> GetCurrentLayoutConfigIds()
    {
        var (parentConfigId, moduleConfigs) = ResolveCurrentLayout();
        var configIds = new List<string> { parentConfigId };

        // 模块名两处来源：静态配置里出现过的（平台布局），以及该租户描述符自带的
        var moduleDataSources = _tenantConnectionResolver.GetModuleDataSourceNames()
            .Concat(moduleConfigs?.Select(moduleConfig => moduleConfig.ModuleDataSource) ?? [])
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Select(name => name.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase);

        foreach (var moduleDataSource in moduleDataSources)
        {
            var moduleConfigId = ModuleDataSourceConfigIds.Build(parentConfigId, moduleDataSource);
            if (_sqlSugarScope.IsAnyConnection(moduleConfigId))
            {
                configIds.Add(moduleConfigId);
            }
        }

        return configIds;
    }

    /// <summary>
    /// 按顺序获取所有库的客户端（初始化/种子数据等场景使用）
    /// </summary>
    /// <returns>各连接的客户端序列</returns>
    public IEnumerable<ISqlSugarClient> GetAllClients()
    {
        foreach (var configId in GetAllConfigIds())
        {
            yield return _sqlSugarScope.GetConnectionScope(configId);
        }
    }

    /// <summary>
    /// 获取底层 SqlSugarScope（仅在需要多库切换/租户管理等高级场景使用）
    /// </summary>
    /// <returns>多连接容器</returns>
    public ITenant AsTenant()
    {
        return _sqlSugarScope;
    }

    /// <summary>
    /// 5.0 下不再把连接钉进工作单元：同一作用域里 SqlSugarScope 对同一 ConfigId 给出同一上下文，
    /// 显式工作单元（SqlSugarUnitOfWorkManager）直接在作用域客户端上开事务
    /// </summary>
    private static ISqlSugarClient EnlistCurrentUnitOfWork(ISqlSugarClient client) => client;
}
