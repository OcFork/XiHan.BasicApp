// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using SqlSugar;

namespace XiHan.BasicApp.Core.Data;

/// <summary>
/// SqlSugar 客户端解析器：按当前调用、实体类型或配置标识取客户端
/// </summary>
/// <remarks>
/// 4.x 由框架按租户与读写分离路由；5.0 的 <c>Persistence.SqlSugar</c> 只提供作用域内的一个客户端，
/// 默认实现 <see cref="ScopedSqlSugarClientResolver"/> 一律返回它，租户分库随 <c>multitenancy.sharding</c> 补齐。
/// </remarks>
public interface ISqlSugarClientResolver
{
    /// <summary>
    /// 当前调用的客户端
    /// </summary>
    ISqlSugarClient GetCurrentClient();

    /// <summary>
    /// 实体类型所在库的客户端
    /// </summary>
    /// <param name="entityType">实体类型</param>
    ISqlSugarClient GetClientForEntity(Type entityType);

    /// <summary>
    /// 实体类型所在库的客户端
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    ISqlSugarClient GetClientForEntity<TEntity>() => GetClientForEntity(typeof(TEntity));

    /// <summary>
    /// 按配置标识取客户端
    /// </summary>
    /// <param name="configId">配置标识</param>
    ISqlSugarClient GetClient(string configId);

    /// <summary>
    /// 全部配置标识
    /// </summary>
    IReadOnlyCollection<string> GetAllConfigIds();

    /// <summary>
    /// 当前布局（租户）下的配置标识
    /// </summary>
    IReadOnlyList<string> GetCurrentLayoutConfigIds();

    /// <summary>
    /// 全部客户端
    /// </summary>
    IEnumerable<ISqlSugarClient> GetAllClients();

    /// <summary>
    /// 多库事务入口
    /// </summary>
    ITenant AsTenant();
}
