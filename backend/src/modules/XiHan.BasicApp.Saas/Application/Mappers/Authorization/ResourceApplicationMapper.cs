// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Entities;

namespace XiHan.BasicApp.Saas.Application.Mappers;

/// <summary>
/// 资源应用层映射器
/// </summary>
public static class ResourceApplicationMapper
{
    /// <summary>
    /// 映射资源创建命令
    /// </summary>
    public static ResourceCreateCommand ToCreateCommand(ResourceCreateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return new ResourceCreateCommand(
            input.ResourceCode,
            input.ResourceName,
            input.ResourceType,
            input.ResourcePath,
            input.Description,
            input.Metadata,
            input.AccessLevel,
            input.Status,
            input.Sort,
            input.Remark);
    }

    /// <summary>
    /// 映射资源列表项
    /// </summary>
    /// <param name="resource">资源定义</param>
    /// <returns>资源列表项 DTO</returns>
    public static ResourceListItemDto ToListItemDto(SysResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);

        return new ResourceListItemDto
        {
            Id = resource.Id,
            ResourceCode = resource.ResourceCode,
            ResourceName = resource.ResourceName,
            ResourceType = resource.ResourceType,
            ResourcePath = resource.ResourcePath,
            Description = resource.Description,
            AccessLevel = resource.AccessLevel,
            IsGlobal = resource.IsGlobal,
            Status = resource.Status,
            Sort = resource.Sort,
            CreatedAt = resource.CreatedAt,
            ModifiedAt = resource.ModifiedAt
        };
    }

    /// <summary>
    /// 映射资源详情
    /// </summary>
    /// <param name="resource">资源定义</param>
    /// <returns>资源详情 DTO</returns>
    public static ResourceDetailDto ToDetailDto(SysResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);

        return new ResourceDetailDto
        {
            Id = resource.Id,
            ResourceCode = resource.ResourceCode,
            ResourceName = resource.ResourceName,
            ResourceType = resource.ResourceType,
            ResourcePath = resource.ResourcePath,
            Description = resource.Description,
            Metadata = resource.Metadata,
            AccessLevel = resource.AccessLevel,
            IsGlobal = resource.IsGlobal,
            Status = resource.Status,
            Sort = resource.Sort,
            Remark = resource.Remark,
            CreatedAt = resource.CreatedAt,
            CreatedId = resource.CreatedId,
            CreatedBy = resource.CreatedBy,
            ModifiedAt = resource.ModifiedAt,
            ModifiedId = resource.ModifiedId,
            ModifiedBy = resource.ModifiedBy
        };
    }

    /// <summary>
    /// 映射资源选择项
    /// </summary>
    /// <param name="resource">资源定义</param>
    /// <returns>资源选择项 DTO</returns>
    public static ResourceSelectItemDto ToSelectItemDto(SysResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);

        return new ResourceSelectItemDto
        {
            Id = resource.Id,
            ResourceCode = resource.ResourceCode,
            ResourceName = resource.ResourceName,
            ResourceType = resource.ResourceType,
            ResourcePath = resource.ResourcePath
        };
    }

    /// <summary>
    /// 映射资源状态变更命令
    /// </summary>
    public static ResourceStatusCommand ToStatusCommand(ResourceStatusUpdateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);
        return new ResourceStatusCommand(input.Id, input.Status, input.Remark);
    }

    /// <summary>
    /// 映射资源更新命令
    /// </summary>
    public static ResourceUpdateCommand ToUpdateCommand(ResourceUpdateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return new ResourceUpdateCommand(
            input.Id,
            input.ResourceName,
            input.ResourceType,
            input.ResourcePath,
            input.Description,
            input.Metadata,
            input.AccessLevel,
            input.Sort,
            input.Remark);
    }
}
