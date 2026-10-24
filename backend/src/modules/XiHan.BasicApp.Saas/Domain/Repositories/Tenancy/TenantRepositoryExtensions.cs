// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Core.Tenancy;
using XiHan.BasicApp.Saas.Domain.Entities;

namespace XiHan.BasicApp.Saas.Domain.Repositories;

/// <summary>
/// 租户仓储扩展：租户标识（字符串）与租户实体主键（long）之间的桥接
/// </summary>
public static class TenantRepositoryExtensions
{
    /// <summary>
    /// 按租户标识取租户；平台或不是数字的标识返回空
    /// </summary>
    /// <param name="repository">租户仓储</param>
    /// <param name="tenantId">租户标识</param>
    /// <param name="cancellationToken">取消令牌</param>
    public static Task<SysTenant?> GetByTenantIdAsync(this ITenantRepository repository, string? tenantId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(repository);
        return TenantIds.ToKey(tenantId) is { } key ? repository.GetByIdAsync(key, cancellationToken) : Task.FromResult<SysTenant?>(null);
    }
}
