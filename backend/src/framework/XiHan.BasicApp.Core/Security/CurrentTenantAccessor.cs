// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Core.Tenancy;

namespace XiHan.BasicApp.Core.Security;

/// <summary>
/// 从执行流环境读当前租户（显式切换优先于调用携带的租户）
/// </summary>
public sealed class CurrentTenantAccessor : ICurrentTenantAccessor
{
    /// <inheritdoc />
    public BasicTenantInfo? Current => TenantAmbient.Id is { } id ? new BasicTenantInfo(id, TenantAmbient.Name) : null;
}
