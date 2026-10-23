// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace XiHan.BasicApp.Saas.Application.Dtos;

/// <summary>
/// 所有权转移 DTO（平台把租户所有者身份转给该租户的另一名成员）
/// </summary>
public sealed class TenantOwnerTransferDto
{
    /// <summary>
    /// 租户主键
    /// </summary>
    // xihan-migrate(V4-02): TenantId 改为字符串；把 Tenant_Id 列类型与按数字比较的地方一起改掉。
    public string TenantId { get; set; } = string.Empty;

    /// <summary>
    /// 接任所有者的成员关系主键
    /// </summary>
    public long MemberId { get; set; }
}
