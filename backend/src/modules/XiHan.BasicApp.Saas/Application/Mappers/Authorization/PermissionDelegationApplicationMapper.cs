// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Entities;

namespace XiHan.BasicApp.Saas.Application.Mappers;

/// <summary>
/// 权限委托应用层映射器
/// </summary>
public static class PermissionDelegationApplicationMapper
{
    /// <summary>
    /// 映射权限委托创建命令
    /// </summary>
    public static PermissionDelegationCreateCommand ToCreateCommand(PermissionDelegationCreateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return new PermissionDelegationCreateCommand(
            input.DelegatorUserId,
            input.DelegateeUserId,
            input.PermissionId,
            input.RoleId,
            input.EffectiveTime,
            input.ExpirationTime,
            input.DelegationReason,
            input.Remark);
    }

    /// <summary>
    /// 映射权限委托列表项
    /// </summary>
    public static PermissionDelegationListItemDto ToListItemDto(
        SysPermissionDelegation delegation,
        SysTenantUser? delegator,
        SysUser? delegatorAccount,
        SysTenantUser? delegatee,
        SysUser? delegateeAccount,
        SysPermission? permission,
        SysRole? role,
        DateTimeOffset now)
    {
        ArgumentNullException.ThrowIfNull(delegation);

        return new PermissionDelegationListItemDto
        {
            Id = delegation.Id,
            DelegatorUserId = delegation.DelegatorUserId,
            DelegatorTenantMemberId = delegator?.Id,
            DelegatorDisplayName = TenantMemberApplicationMapper.ToDisplayName(delegator, delegatorAccount),
            DelegateeUserId = delegation.DelegateeUserId,
            DelegateeTenantMemberId = delegatee?.Id,
            DelegateeDisplayName = TenantMemberApplicationMapper.ToDisplayName(delegatee, delegateeAccount),
            PermissionId = delegation.PermissionId,
            PermissionCode = permission?.PermissionCode,
            PermissionName = permission?.PermissionName,
            RoleId = delegation.RoleId,
            RoleCode = role?.RoleCode,
            RoleName = role?.RoleName,
            DelegationStatus = delegation.DelegationStatus,
            EffectiveTime = delegation.EffectiveTime,
            ExpirationTime = delegation.ExpirationTime,
            IsExpired = delegation.ExpirationTime <= now,
            DelegationReason = delegation.DelegationReason,
            CreatedAt = delegation.CreatedAt,
            ModifiedAt = delegation.ModifiedAt
        };
    }

    /// <summary>
    /// 映射权限委托详情
    /// </summary>
    public static PermissionDelegationDetailDto ToDetailDto(
        SysPermissionDelegation delegation,
        SysTenantUser? delegator,
        SysUser? delegatorAccount,
        SysTenantUser? delegatee,
        SysUser? delegateeAccount,
        SysPermission? permission,
        SysRole? role,
        DateTimeOffset now)
    {
        ArgumentNullException.ThrowIfNull(delegation);

        return new PermissionDelegationDetailDto
        {
            Id = delegation.Id,
            DelegatorUserId = delegation.DelegatorUserId,
            DelegatorTenantMemberId = delegator?.Id,
            DelegatorDisplayName = TenantMemberApplicationMapper.ToDisplayName(delegator, delegatorAccount),
            DelegateeUserId = delegation.DelegateeUserId,
            DelegateeTenantMemberId = delegatee?.Id,
            DelegateeDisplayName = TenantMemberApplicationMapper.ToDisplayName(delegatee, delegateeAccount),
            PermissionId = delegation.PermissionId,
            PermissionCode = permission?.PermissionCode,
            PermissionName = permission?.PermissionName,
            PermissionDescription = permission?.PermissionDescription,
            RoleId = delegation.RoleId,
            RoleCode = role?.RoleCode,
            RoleName = role?.RoleName,
            RoleDescription = role?.RoleDescription,
            DelegationStatus = delegation.DelegationStatus,
            EffectiveTime = delegation.EffectiveTime,
            ExpirationTime = delegation.ExpirationTime,
            IsExpired = delegation.ExpirationTime <= now,
            DelegationReason = delegation.DelegationReason,
            Remark = delegation.Remark,
            CreatedAt = delegation.CreatedAt,
            CreatedId = delegation.CreatedId,
            CreatedBy = delegation.CreatedBy,
            ModifiedAt = delegation.ModifiedAt,
            ModifiedId = delegation.ModifiedId,
            ModifiedBy = delegation.ModifiedBy
        };
    }

    /// <summary>
    /// 映射权限委托状态变更命令
    /// </summary>
    public static PermissionDelegationStatusCommand ToStatusCommand(PermissionDelegationStatusUpdateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);
        return new PermissionDelegationStatusCommand(input.Id, input.DelegationStatus, input.Remark);
    }

    /// <summary>
    /// 映射权限委托更新命令
    /// </summary>
    public static PermissionDelegationUpdateCommand ToUpdateCommand(PermissionDelegationUpdateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return new PermissionDelegationUpdateCommand(
            input.Id,
            input.DelegatorUserId,
            input.DelegateeUserId,
            input.PermissionId,
            input.RoleId,
            input.EffectiveTime,
            input.ExpirationTime,
            input.DelegationReason,
            input.Remark);
    }
}
