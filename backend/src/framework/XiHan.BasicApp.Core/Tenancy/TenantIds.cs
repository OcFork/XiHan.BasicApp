// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace XiHan.BasicApp.Core.Tenancy;

/// <summary>
/// 租户标识约定：平台就是 0 号租户，空白与 <c>"0"</c> 同义；比较前先规范化
/// </summary>
public static class TenantIds
{
    /// <summary>
    /// 平台（0 号租户）的标识
    /// </summary>
    public const string Platform = "0";

    /// <summary>
    /// 是否平台：空白或 <c>"0"</c>
    /// </summary>
    /// <param name="tenantId">租户标识</param>
    public static bool IsPlatform(string? tenantId) => string.IsNullOrWhiteSpace(tenantId) || tenantId.Trim() == Platform;

    /// <summary>
    /// 规范化：平台统一为 <see cref="Platform"/>，其余去掉首尾空白
    /// </summary>
    /// <param name="tenantId">租户标识</param>
    public static string Normalize(string? tenantId) => IsPlatform(tenantId) ? Platform : tenantId!.Trim();

    /// <summary>
    /// 规范化后是否同一租户
    /// </summary>
    /// <param name="left">租户标识</param>
    /// <param name="right">租户标识</param>
    public static bool AreSame(string? left, string? right) => string.Equals(Normalize(left), Normalize(right), StringComparison.Ordinal);
}
