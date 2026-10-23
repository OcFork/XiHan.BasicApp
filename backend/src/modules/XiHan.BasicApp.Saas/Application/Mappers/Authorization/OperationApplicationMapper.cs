// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Entities;

namespace XiHan.BasicApp.Saas.Application.Mappers;

/// <summary>
/// 操作应用层映射器
/// </summary>
public static class OperationApplicationMapper
{
    /// <summary>
    /// 映射操作创建命令
    /// </summary>
    public static OperationCreateCommand ToCreateCommand(OperationCreateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return new OperationCreateCommand(
            input.OperationCode,
            input.OperationName,
            input.OperationTypeCode,
            input.Category,
            input.HttpMethod,
            input.Description,
            input.Icon,
            input.Color,
            input.IsDangerous,
            input.IsRequireAudit,
            input.Status,
            input.Sort,
            input.Remark);
    }

    /// <summary>
    /// 映射操作列表项
    /// </summary>
    /// <param name="operation">操作定义</param>
    /// <returns>操作列表项 DTO</returns>
    public static OperationListItemDto ToListItemDto(SysOperation operation)
    {
        ArgumentNullException.ThrowIfNull(operation);

        return new OperationListItemDto
        {
            Id = operation.Id,
            OperationCode = operation.OperationCode,
            OperationName = operation.OperationName,
            OperationTypeCode = operation.OperationTypeCode,
            Category = operation.Category,
            HttpMethod = operation.HttpMethod,
            Description = operation.Description,
            Icon = operation.Icon,
            Color = operation.Color,
            IsDangerous = operation.IsDangerous,
            IsRequireAudit = operation.IsRequireAudit,
            IsGlobal = operation.IsGlobal,
            Status = operation.Status,
            Sort = operation.Sort,
            CreatedAt = operation.CreatedAt,
            ModifiedAt = operation.ModifiedAt
        };
    }

    /// <summary>
    /// 映射操作详情
    /// </summary>
    /// <param name="operation">操作定义</param>
    /// <returns>操作详情 DTO</returns>
    public static OperationDetailDto ToDetailDto(SysOperation operation)
    {
        ArgumentNullException.ThrowIfNull(operation);

        return new OperationDetailDto
        {
            Id = operation.Id,
            OperationCode = operation.OperationCode,
            OperationName = operation.OperationName,
            OperationTypeCode = operation.OperationTypeCode,
            Category = operation.Category,
            HttpMethod = operation.HttpMethod,
            Description = operation.Description,
            Icon = operation.Icon,
            Color = operation.Color,
            IsDangerous = operation.IsDangerous,
            IsRequireAudit = operation.IsRequireAudit,
            IsGlobal = operation.IsGlobal,
            Status = operation.Status,
            Sort = operation.Sort,
            Remark = operation.Remark,
            CreatedAt = operation.CreatedAt,
            CreatedId = operation.CreatedId,
            CreatedBy = operation.CreatedBy,
            ModifiedAt = operation.ModifiedAt,
            ModifiedId = operation.ModifiedId,
            ModifiedBy = operation.ModifiedBy
        };
    }

    /// <summary>
    /// 映射操作选择项
    /// </summary>
    /// <param name="operation">操作定义</param>
    /// <returns>操作选择项 DTO</returns>
    public static OperationSelectItemDto ToSelectItemDto(SysOperation operation)
    {
        ArgumentNullException.ThrowIfNull(operation);

        return new OperationSelectItemDto
        {
            Id = operation.Id,
            OperationCode = operation.OperationCode,
            OperationName = operation.OperationName,
            OperationTypeCode = operation.OperationTypeCode,
            Category = operation.Category,
            HttpMethod = operation.HttpMethod,
            IsDangerous = operation.IsDangerous,
            IsRequireAudit = operation.IsRequireAudit
        };
    }

    /// <summary>
    /// 映射操作状态变更命令
    /// </summary>
    public static OperationStatusCommand ToStatusCommand(OperationStatusUpdateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);
        return new OperationStatusCommand(input.Id, input.Status, input.Remark);
    }

    /// <summary>
    /// 映射操作更新命令
    /// </summary>
    public static OperationUpdateCommand ToUpdateCommand(OperationUpdateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return new OperationUpdateCommand(
            input.Id,
            input.OperationName,
            input.OperationTypeCode,
            input.Category,
            input.HttpMethod,
            input.Description,
            input.Icon,
            input.Color,
            input.IsDangerous,
            input.IsRequireAudit,
            input.Sort,
            input.Remark);
    }
}
