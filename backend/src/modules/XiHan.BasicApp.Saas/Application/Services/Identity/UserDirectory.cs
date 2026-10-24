// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Enums;
using XiHan.BasicApp.Saas.Domain.Repositories;
using XiHan.BasicApp.Core.Security;
using XiHan.BasicApp.Core.Paging.Dtos;
using XiHan.BasicApp.Core.Tenancy;

namespace XiHan.BasicApp.Saas.Application.Services;

/// <summary>
/// 当前上下文的用户目录实现
/// </summary>
public sealed class UserDirectory(ICurrentTenant currentTenant, IUserRepository userRepository) : IUserDirectory
{
    /// <summary>
    /// 当前上下文可见用户的分页
    /// </summary>
    public Task<PageResultDtoBase<SysUser>> GetPagedAsync(PageRequestDtoBase request, CancellationToken cancellationToken = default)
    {
        var scope = CurrentScope;
        return scope == TenantIds.Platform
            ? userRepository.GetPagedAsync(request, cancellationToken)
            : userRepository.GetMemberAccountsPagedAsync(scope, request, cancellationToken);
    }

    /// <summary>
    /// 当前上下文可见的用户，不可见时返回 null
    /// </summary>
    public Task<SysUser?> FindAsync(long userId, CancellationToken cancellationToken = default)
    {
        var scope = CurrentScope;
        return scope == TenantIds.Platform
            ? userRepository.GetByIdAsync(userId, cancellationToken)
            : userRepository.GetMemberAccountAsync(scope, userId, cancellationToken);
    }

    /// <summary>
    /// 当前上下文可见用户里启用账号的主键
    /// </summary>
    public async Task<IReadOnlyList<long>> GetEnabledIdsAsync(CancellationToken cancellationToken = default)
    {
        var scope = CurrentScope;
        if (scope != TenantIds.Platform)
        {
            return await userRepository.GetEnabledMemberAccountIdsAsync(scope, cancellationToken);
        }

        var accounts = await userRepository.GetListAsync(user => user.Status == EnableStatus.Enabled, cancellationToken);
        return [.. accounts.Select(user => user.Id)];
    }

    /// <summary>
    /// 是否为当前上下文注册的账号
    /// </summary>
    public bool IsHomeAccount(SysUser user)
    {
        ArgumentNullException.ThrowIfNull(user);
        return TenantIds.AreSame(user.TenantId, CurrentScope);
    }

    /// <summary>
    /// 当前上下文（平台为 "0"）
    /// </summary>
    private string CurrentScope => TenantIds.Normalize(currentTenant.Id);
}
