// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Entities;

namespace XiHan.BasicApp.Saas.Application.Mappers;

/// <summary>
/// 角色应用层映射器
/// </summary>
public static class RoleApplicationMapper
{
    /// <summary>
    /// 映射角色创建命令
    /// </summary>
    public static RoleCreateCommand ToCreateCommand(RoleCreateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return new RoleCreateCommand(
            input.RoleCode,
            input.RoleName,
            input.RoleDescription,
            input.RoleType,
            input.MaxMembers,
            input.Status,
            input.Sort,
            input.Remark);
    }

    /// <summary>
    /// 映射角色列表项
    /// </summary>
    /// <param name="role">角色定义</param>
    /// <returns>角色列表项 DTO</returns>
    public static RoleListItemDto ToListItemDto(SysRole role)
    {
        ArgumentNullException.ThrowIfNull(role);

        return new RoleListItemDto
        {
            Id = role.Id,
            RoleCode = role.RoleCode,
            RoleName = role.RoleName,
            RoleDescription = role.RoleDescription,
            RoleType = role.RoleType,
            IsGlobal = role.IsGlobal,
            DataScope = role.DataScope,
            MaxMembers = role.MaxMembers,
            Status = role.Status,
            Sort = role.Sort,
            CreatedAt = role.CreatedAt,
            ModifiedAt = role.ModifiedAt
        };
    }

    /// <summary>
    /// 映射角色详情
    /// </summary>
    /// <param name="role">角色定义</param>
    /// <returns>角色详情 DTO</returns>
    public static RoleDetailDto ToDetailDto(SysRole role)
    {
        ArgumentNullException.ThrowIfNull(role);

        return new RoleDetailDto
        {
            Id = role.Id,
            RoleCode = role.RoleCode,
            RoleName = role.RoleName,
            RoleDescription = role.RoleDescription,
            RoleType = role.RoleType,
            IsGlobal = role.IsGlobal,
            DataScope = role.DataScope,
            MaxMembers = role.MaxMembers,
            Status = role.Status,
            Sort = role.Sort,
            Remark = role.Remark,
            CreatedAt = role.CreatedAt,
            CreatedId = role.CreatedId,
            CreatedBy = role.CreatedBy,
            ModifiedAt = role.ModifiedAt,
            ModifiedId = role.ModifiedId,
            ModifiedBy = role.ModifiedBy
        };
    }

    /// <summary>
    /// 映射角色选择项
    /// </summary>
    /// <param name="role">角色定义</param>
    /// <returns>角色选择项 DTO</returns>
    public static RoleSelectItemDto ToSelectItemDto(SysRole role)
    {
        ArgumentNullException.ThrowIfNull(role);

        return new RoleSelectItemDto
        {
            Id = role.Id,
            RoleCode = role.RoleCode,
            RoleName = role.RoleName,
            RoleType = role.RoleType,
            IsGlobal = role.IsGlobal
        };
    }

    /// <summary>
    /// 映射角色状态变更命令
    /// </summary>
    public static RoleStatusChangeCommand ToStatusCommand(RoleStatusUpdateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);
        return new RoleStatusChangeCommand(input.Id, input.Status, input.Remark);
    }

    /// <summary>
    /// 映射角色更新命令
    /// </summary>
    public static RoleUpdateCommand ToUpdateCommand(RoleUpdateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return new RoleUpdateCommand(
            input.Id,
            input.RoleName,
            input.RoleDescription,
            input.RoleType,
            input.MaxMembers,
            input.Sort,
            input.Remark);
    }
}
