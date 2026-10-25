// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using SqlSugar;
using XiHan.BasicApp.Core.Data.SqlSugar.Options;
using XiHan.BasicApp.Core.Data.SqlSugar.Tenanting;
using XiHan.Framework.MultiTenancy.Abstractions.Sharding;

namespace XiHan.BasicApp.Core.Tests;

/// <summary>
/// 5.0 租户分片到 SqlSugar 租户连接描述符的换算：ConfigId 带前缀、提供者名映射数据库类型、缺失或未知的提供者名 fail-closed。
/// </summary>
public sealed class TenantShardConnectionsTests
{
    [Theory]
    [InlineData("postgresql", DbType.PostgreSQL)]
    [InlineData("mysql", DbType.MySql)]
    [InlineData("sqlserver", DbType.SqlServer)]
    [InlineData("sqlite", DbType.Sqlite)]
    [InlineData("oracle", DbType.Oracle)]
    public void ToConnection_PrefixesConfigId_AndMapsProvider(string provider, DbType expected)
    {
        var shard = new TenantShard("42", "Host=tenant-42;Database=app", provider);

        var connection = TenantShardConnections.ToConnection(shard, new XiHanSqlSugarCoreOptions { TenantConfigIdPrefix = "Tenant_" });

        Assert.Equal("Tenant_42", connection.ConfigId);
        Assert.Equal("Host=tenant-42;Database=app", connection.ConnectionString);
        Assert.Equal(expected, connection.DbType);
        Assert.Null(connection.ModuleDataSourceConfigs);
    }

    [Fact]
    public void ToConnection_RejectsMissingOrUnknownProvider()
    {
        var options = new XiHanSqlSugarCoreOptions();

        _ = Assert.Throws<InvalidOperationException>(() => TenantShardConnections.ToConnection(new TenantShard("7", "Host=x"), options));
        _ = Assert.Throws<InvalidOperationException>(() => TenantShardConnections.ToConnection(new TenantShard("7", "Host=x", "db2"), options));
    }
}
