// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Entities;

namespace XiHan.BasicApp.Saas.Application.Mappers;

/// <summary>
/// 岗位应用层映射器
/// </summary>
public static class PositionApplicationMapper
{
    /// <summary>
    /// 映射岗位创建命令
    /// </summary>
    public static PositionCreateCommand ToCreateCommand(PositionCreateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return new PositionCreateCommand(
            input.PositionCode,
            input.PositionName,
            input.Status,
            input.Sort,
            input.Remark);
    }

    /// <summary>
    /// 映射岗位更新命令
    /// </summary>
    public static PositionUpdateCommand ToUpdateCommand(PositionUpdateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return new PositionUpdateCommand(
            input.Id,
            input.PositionName,
            input.Sort,
            input.Remark);
    }

    /// <summary>
    /// 映射岗位状态命令
    /// </summary>
    public static PositionStatusChangeCommand ToStatusCommand(PositionStatusUpdateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return new PositionStatusChangeCommand(input.Id, input.Status, input.Remark);
    }

    /// <summary>
    /// 映射岗位列表项
    /// </summary>
    /// <param name="position">岗位实体</param>
    /// <returns>岗位列表项 DTO</returns>
    public static PositionListItemDto ToListItemDto(SysPosition position)
    {
        ArgumentNullException.ThrowIfNull(position);

        return new PositionListItemDto
        {
            Id = position.Id,
            PositionCode = position.PositionCode,
            PositionName = position.PositionName,
            Status = position.Status,
            Sort = position.Sort,
            CreatedAt = position.CreatedAt,
            ModifiedAt = position.ModifiedAt
        };
    }

    /// <summary>
    /// 映射岗位详情
    /// </summary>
    /// <param name="position">岗位实体</param>
    /// <returns>岗位详情 DTO</returns>
    public static PositionDetailDto ToDetailDto(SysPosition position)
    {
        ArgumentNullException.ThrowIfNull(position);

        return new PositionDetailDto
        {
            Id = position.Id,
            PositionCode = position.PositionCode,
            PositionName = position.PositionName,
            Status = position.Status,
            Sort = position.Sort,
            Remark = position.Remark,
            CreatedAt = position.CreatedAt,
            CreatedId = position.CreatedId,
            CreatedBy = position.CreatedBy,
            ModifiedAt = position.ModifiedAt,
            ModifiedId = position.ModifiedId,
            ModifiedBy = position.ModifiedBy
        };
    }
}
