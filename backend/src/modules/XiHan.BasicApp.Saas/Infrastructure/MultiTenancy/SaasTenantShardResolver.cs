// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Concurrent;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using XiHan.BasicApp.Core.Data;
using XiHan.BasicApp.Core.Security;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.Framework.MultiTenancy.Abstractions;
using XiHan.Framework.MultiTenancy.Abstractions.Sharding;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Infrastructure.MultiTenancy;

/// <summary>
/// SaaS 租户分片解析器（库隔离桥接：SysTenant → 5.0 租户分片）
/// </summary>
/// <remarks>
/// 实现 5.0 的 <c>multitenancy.sharding@1</c> 契约，顶替画像里按租户属性推分片的默认解析器：
/// Tenant 阶段在租户进入安全上下文后调用一次并把分片写进调用上下文，SqlSugar 客户端解析器据此按租户建连。
/// <list type="bullet">
///   <item><see cref="TenantIsolationMode.Field"/>：无分片，走平台库 + 行过滤器（现状）；</item>
///   <item><see cref="TenantIsolationMode.Database"/>：解密连接串，分片的提供者名按 <see cref="TenantDatabaseType"/> 给出，Core 据此建 <c>Tenant_{id}</c> 连接；</item>
///   <item><see cref="TenantIsolationMode.Schema"/>：抛异常（尚未实装，fail-closed，杜绝静默退化）。</item>
/// </list>
/// 读取租户元数据时切到平台作用域（<c>ICurrentTenant.Change(null)</c>）：租户注册表是平台数据（<c>TenantId=0</c>），
/// 在平台作用域走默认连接且不会递归回本解析器；结果按租户缓存（含「无分片」），隔离配置变更须经 <see cref="ITenantConnectionCacheInvalidator"/> 失效。
/// 读取失败不回退默认连接：库隔离租户退回平台库会把它的数据写进平台库，失败以 <c>MULTITENANCY_UNAVAILABLE_002</c> 返回（fail-closed，不入缓存，下次请求重试）。
/// <para>
/// 本类只负责租户维度（这个租户有没有独立主库）。库隔离租户的模块库由 Core 按约定派生
/// （<c>XiHan:Data:SqlSugarCore:EnableTenantModuleDatabaseConvention</c>）：库名从租户主库名派生为
/// <c>{租户库名}_{模块名}</c>，与主库一并建连。
/// </para>
/// </remarks>
public sealed class SaasTenantShardResolver : ITenantShardResolver, ITenantConnectionCacheInvalidator
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ITenantConnectionSecretProtector _secretProtector;

    // 值为无值表示「该租户无需独立连接」的已缓存结论；失败不入缓存，下次请求重试（保持 fail-closed）
    private readonly ConcurrentDictionary<string, Maybe<TenantShard>> _cache = new(StringComparer.Ordinal);

    /// <summary>
    /// 构造函数
    /// </summary>
    public SaasTenantShardResolver(IServiceScopeFactory scopeFactory, ITenantConnectionSecretProtector secretProtector)
    {
        _scopeFactory = scopeFactory;
        _secretProtector = secretProtector;
    }

    /// <summary>
    /// 解析指定租户的分片：库隔离租户返回连接串与数据库类型，其余租户无值
    /// </summary>
    /// <param name="tenantId">租户标识</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>分片；无需独立连接时为无值；租户目录读取失败时为 <c>MULTITENANCY_UNAVAILABLE_002</c></returns>
    public async ValueTask<Result<Maybe<TenantShard>>> ResolveAsync(string tenantId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantId);
        tenantId = tenantId.Trim();
        if (_cache.TryGetValue(tenantId, out var cached))
        {
            return Result.Success(cached);
        }

        SysTenant? tenant;
        try
        {
            tenant = await LoadTenantAsync(tenantId, cancellationToken);
        }
        catch (Exception exception) when (exception is not OperationCanceledException and not NotSupportedException and not InvalidOperationException)
        {
            // 租户目录不可达：不缓存、不降级，让这次调用失败
            return Error.Unavailable(MultiTenancyErrors.ShardUnavailable, MultiTenancyMessages.ShardUnavailable, MessageArguments.Of(("tenantId", tenantId), ("reason", exception.Message)));
        }

        var shard = ToShard(tenantId, tenant);
        _cache[tenantId] = shard;
        return Result.Success(shard);
    }

    /// <summary>
    /// 丢弃缓存的分片；<paramref name="tenantId"/> 为空清全部
    /// </summary>
    /// <param name="tenantId">租户标识</param>
    public void Invalidate(string? tenantId = null)
    {
        if (tenantId is null)
        {
            _cache.Clear();
        }
        else
        {
            _cache.TryRemove(tenantId.Trim(), out _);
        }
    }

    /// <summary>
    /// 使指定租户的连接缓存失效
    /// </summary>
    /// <param name="tenantId">租户标识</param>
    public void Invalidate(long tenantId)
    {
        Invalidate(tenantId.ToString(CultureInfo.InvariantCulture));
    }

    private async Task<SysTenant?> LoadTenantAsync(string tenantId, CancellationToken cancellationToken)
    {
        // 租户标识是租户实体主键的字符串形式；不是数字就按 fail-closed 拒绝
        if (!long.TryParse(tenantId, NumberStyles.None, CultureInfo.InvariantCulture, out var tenantKey))
        {
            throw new InvalidOperationException($"租户标识 {tenantId} 不是有效的租户主键（fail-closed）。");
        }

        using var scope = _scopeFactory.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var currentTenant = serviceProvider.GetRequiredService<ICurrentTenant>();
        // 租户目录固定在平台库，按实体解析不经过本解析器，不会递归；平台作用域读取租户元数据
        using (currentTenant.Change(null))
        {
            var client = serviceProvider.GetRequiredService<ISqlSugarClientResolver>().GetClientForEntity<SysTenant>();
            return await client.Queryable<SysTenant>().Where(x => x.Id == tenantKey).FirstAsync(cancellationToken);
        }
    }

    private Maybe<TenantShard> ToShard(string tenantId, SysTenant? tenant)
    {
        if (tenant is null)
        {
            return Maybe.None<TenantShard>();
        }

        switch (tenant.IsolationMode)
        {
            case TenantIsolationMode.Field:
                return Maybe.None<TenantShard>();
            case TenantIsolationMode.Schema:
                throw new NotSupportedException($"租户 {tenantId} 声明 Schema 隔离，但当前尚未实装 Schema 隔离（fail-closed，拒绝退化为行隔离）。");
            case TenantIsolationMode.Database:
                var connectionString = _secretProtector.Unprotect(tenant.ConnectionString);
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new InvalidOperationException($"租户 {tenantId} 声明库隔离但连接字符串为空（fail-closed）。");
                }

                return Maybe.Some(new TenantShard(tenantId, connectionString, MapProvider(tenant.DatabaseType)));
            default:
                return Maybe.None<TenantShard>();
        }
    }

    /// <summary>
    /// 租户的数据库类型映射为分片的提供者名（小写，Core 的 <c>TenantShardConnections</c> 按它选 SqlSugar 的 DbType）
    /// </summary>
    private static string MapProvider(TenantDatabaseType? databaseType)
    {
        return databaseType switch
        {
            TenantDatabaseType.SqlServer => "sqlserver",
            TenantDatabaseType.MySql => "mysql",
            TenantDatabaseType.PostgreSql => "postgresql",
            TenantDatabaseType.SQLite => "sqlite",
            TenantDatabaseType.Oracle => "oracle",
            _ => throw new InvalidOperationException("库隔离租户必须指定有效的数据库类型（DatabaseType）。")
        };
    }
}
