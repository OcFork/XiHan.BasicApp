// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Entities;

namespace XiHan.BasicApp.Saas.Application.Mappers;

/// <summary>
/// 用户部门归属应用层映射器
/// </summary>
public static class UserDepartmentApplicationMapper
{
    /// <summary>
    /// 映射用户部门归属列表项
    /// </summary>
    /// <param name="userDepartment">用户部门归属</param>
    /// <param name="department">部门</param>
    /// <param name="position">岗位（用于回填岗位名称，可空）</param>
    /// <returns>用户部门归属列表项 DTO</returns>
    public static UserDepartmentListItemDto ToListItemDto(SysUserDepartment userDepartment, SysDepartment? department, SysPosition? position = null)
    {
        ArgumentNullException.ThrowIfNull(userDepartment);

        return new UserDepartmentListItemDto
        {
            Id = userDepartment.Id,
            UserId = userDepartment.UserId,
            DepartmentId = userDepartment.DepartmentId,
            DepartmentCode = department?.DepartmentCode,
            DepartmentName = department?.DepartmentName,
            ParentId = department?.ParentId,
            DepartmentType = department?.DepartmentType,
            DepartmentStatus = department?.Status,
            PositionId = userDepartment.PositionId,
            PositionName = position?.PositionName,
            JobNumber = userDepartment.JobNumber,
            JobLevel = userDepartment.JobLevel,
            JoinTime = userDepartment.JoinTime,
            IsMain = userDepartment.IsMain,
            Status = userDepartment.Status,
            Remark = userDepartment.Remark,
            CreatedAt = userDepartment.CreatedAt
        };
    }

    /// <summary>
    /// 映射用户部门归属详情
    /// </summary>
    /// <param name="userDepartment">用户部门归属</param>
    /// <param name="department">部门</param>
    /// <param name="position">岗位（用于回填岗位名称，可空）</param>
    /// <returns>用户部门归属详情 DTO</returns>
    public static UserDepartmentDetailDto ToDetailDto(SysUserDepartment userDepartment, SysDepartment? department, SysPosition? position = null)
    {
        ArgumentNullException.ThrowIfNull(userDepartment);

        return new UserDepartmentDetailDto
        {
            Id = userDepartment.Id,
            UserId = userDepartment.UserId,
            DepartmentId = userDepartment.DepartmentId,
            DepartmentCode = department?.DepartmentCode,
            DepartmentName = department?.DepartmentName,
            ParentId = department?.ParentId,
            DepartmentType = department?.DepartmentType,
            DepartmentStatus = department?.Status,
            PositionId = userDepartment.PositionId,
            PositionName = position?.PositionName,
            JobNumber = userDepartment.JobNumber,
            JobLevel = userDepartment.JobLevel,
            JoinTime = userDepartment.JoinTime,
            IsMain = userDepartment.IsMain,
            Status = userDepartment.Status,
            Remark = userDepartment.Remark,
            CreatedAt = userDepartment.CreatedAt,
            CreatedId = userDepartment.CreatedId,
            CreatedBy = userDepartment.CreatedBy
        };
    }

    /// <summary>
    /// 映射用户部门归属状态变更命令
    /// </summary>
    public static UserDepartmentStatusChangeCommand ToStatusCommand(UserDepartmentStatusUpdateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);
        return new UserDepartmentStatusChangeCommand(input.Id, input.Status, input.Remark);
    }

    /// <summary>
    /// 映射用户部门归属更新命令
    /// </summary>
    public static UserDepartmentUpdateCommand ToUpdateCommand(UserDepartmentUpdateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);
        return new UserDepartmentUpdateCommand(
            input.Id,
            input.IsMain,
            input.Remark,
            input.PositionId,
            input.JobNumber,
            input.JobLevel,
            input.JoinTime);
    }
}
