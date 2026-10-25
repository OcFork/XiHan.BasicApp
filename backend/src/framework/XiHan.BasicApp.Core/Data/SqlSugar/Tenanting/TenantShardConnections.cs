// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using SqlSugar;
using XiHan.BasicApp.Core.Data.SqlSugar.Options;
using XiHan.Framework.MultiTenancy.Abstractions.Sharding;

namespace XiHan.BasicApp.Core.Data.SqlSugar.Tenanting;

/// <summary>
/// 把 5.0 的租户分片（<see cref="TenantShard"/>）换算成 SqlSugar 的租户连接描述符
/// </summary>
/// <remarks>
/// 分片由 <c>ITenantShardResolver</c>（Saas 按 <c>SysTenant</c> 实现）解析，Tenant 阶段写进调用上下文；
/// 这里只做形状转换：ConfigId 按 <see cref="XiHanSqlSugarCoreOptions.TenantConfigIdPrefix"/> 拼出，
/// 提供者名（小写）映射为 <see cref="DbType"/>。库隔离租户没有给出有效的提供者名时 fail-closed 抛出，
/// 绝不猜一种数据库类型去连。
/// </remarks>
public static class TenantShardConnections
{
    /// <summary>
    /// 换算为租户连接描述符
    /// </summary>
    /// <param name="shard">租户分片</param>
    /// <param name="options">SqlSugarCore 选项（提供 ConfigId 前缀）</param>
    /// <returns>租户连接描述符</returns>
    public static SqlSugarTenantConnection ToConnection(TenantShard shard, XiHanSqlSugarCoreOptions options)
    {
        ArgumentNullException.ThrowIfNull(shard);
        ArgumentNullException.ThrowIfNull(options);
        return new SqlSugarTenantConnection($"{options.TenantConfigIdPrefix}{shard.TenantId}", shard.ConnectionString, MapDbType(shard.Provider));
    }

    /// <summary>
    /// 提供者名映射为 SqlSugar 数据库类型；空或未知的名字按 fail-closed 拒绝
    /// </summary>
    /// <param name="provider">分片的提供者名（小写）</param>
    /// <returns>数据库类型</returns>
    public static DbType MapDbType(string? provider)
    {
        return provider switch
        {
            "sqlserver" => DbType.SqlServer,
            "mysql" => DbType.MySql,
            "postgresql" => DbType.PostgreSQL,
            "sqlite" => DbType.Sqlite,
            "oracle" => DbType.Oracle,
            _ => throw new InvalidOperationException($"库隔离租户必须指定有效的数据库类型（DatabaseType），当前为 '{provider}'。")
        };
    }
}
