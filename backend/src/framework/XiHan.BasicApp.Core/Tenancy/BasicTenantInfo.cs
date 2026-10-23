// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace XiHan.BasicApp.Core.Tenancy;

/// <summary>
/// 基本租户信息（5.0 的租户标识是字符串）
/// </summary>
/// <param name="tenantId">租户标识</param>
/// <param name="name">租户名称</param>
public class BasicTenantInfo(string? tenantId, string? name = null)
{
    /// <summary>
    /// 租户标识
    /// </summary>
    public string? TenantId { get; } = tenantId;

    /// <summary>
    /// 租户名称
    /// </summary>
    public string? Name { get; } = name;
}
