// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using SqlSugar;
using XiHan.BasicApp.Core.Security;

namespace XiHan.BasicApp.Core.Data;

/// <summary>
/// 单库解析器：所有入口都返回 5.0 持久化特性在当前作用域里注册的那个客户端
/// </summary>
/// <param name="client">作用域内的 SqlSugar 客户端</param>
/// <param name="currentTenant">当前租户</param>
public sealed class ScopedSqlSugarClientResolver(ISqlSugarClient client, ICurrentTenant currentTenant) : ISqlSugarClientResolver
{
    /// <inheritdoc />
    public string? CurrentTenantId => currentTenant.Id;

    /// <inheritdoc />
    public ISqlSugarClient GetCurrentClient() => client;

    /// <inheritdoc />
    public ISqlSugarClient GetClientForEntity(Type entityType) => client;

    /// <inheritdoc />
    public ISqlSugarClient GetClient(string configId) => client;

    /// <inheritdoc />
    public IReadOnlyCollection<string> GetAllConfigIds() => [ConfigId];

    /// <inheritdoc />
    public IReadOnlyList<string> GetCurrentLayoutConfigIds() => [ConfigId];

    /// <inheritdoc />
    public IEnumerable<ISqlSugarClient> GetAllClients() => [client];

    /// <inheritdoc />
    public ITenant AsTenant() => client.AsTenant();

    /// <summary>
    /// 当前连接的配置标识；没配置时为空串
    /// </summary>
    private string ConfigId => client.CurrentConnectionConfig?.ConfigId?.ToString() ?? string.Empty;
}
