// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Core.Security;

namespace XiHan.BasicApp.Core.Data.SqlSugar.Extensions;

/// <summary>
/// 实体审计赋值上下文
/// </summary>
public readonly record struct EntityAuditContext(long? UserId, string? UserName, string? TenantId)
{
    /// <summary>
    /// 从当前用户与租户上下文创建审计上下文
    /// </summary>
    /// <param name="currentUser">当前用户</param>
    /// <param name="tenantId">当前租户标识</param>
    /// <returns></returns>
    public static EntityAuditContext From(ICurrentUser? currentUser, string? tenantId)
    {
        if (currentUser?.IsAuthenticated != true)
        {
            return new EntityAuditContext(null, null, tenantId);
        }

        return new EntityAuditContext(currentUser.UserId, currentUser.UserName, tenantId);
    }
}
