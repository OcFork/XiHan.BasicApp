// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Entities;

namespace XiHan.BasicApp.Saas.Application.Mappers;

/// <summary>
/// 租户版本应用层映射器
/// </summary>
public static class TenantEditionApplicationMapper
{
    /// <summary>
    /// 映射租户版本创建命令
    /// </summary>
    public static TenantEditionCreateCommand ToCreateCommand(TenantEditionCreateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return new TenantEditionCreateCommand(
            input.EditionCode,
            input.EditionName,
            input.Description,
            input.UserLimit,
            input.StorageLimit,
            input.Price,
            input.BillingPeriodMonths,
            input.IsFree,
            input.IsDefault,
            input.Status,
            input.Sort,
            input.Remark);
    }

    /// <summary>
    /// 映射租户版本更新命令
    /// </summary>
    public static TenantEditionUpdateCommand ToUpdateCommand(TenantEditionUpdateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return new TenantEditionUpdateCommand(
            input.Id,
            input.EditionName,
            input.Description,
            input.UserLimit,
            input.StorageLimit,
            input.Price,
            input.BillingPeriodMonths,
            input.IsFree,
            input.IsDefault,
            input.Sort,
            input.Remark);
    }

    /// <summary>
    /// 映射租户版本默认变更命令
    /// </summary>
    public static TenantEditionDefaultChangeCommand ToDefaultCommand(TenantEditionDefaultUpdateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return new TenantEditionDefaultChangeCommand(input.Id);
    }

    /// <summary>
    /// 映射租户版本状态命令
    /// </summary>
    public static TenantEditionStatusChangeCommand ToStatusCommand(TenantEditionStatusUpdateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return new TenantEditionStatusChangeCommand(input.Id, input.Status);
    }

    /// <summary>
    /// 映射租户版本列表项
    /// </summary>
    /// <param name="edition">租户版本</param>
    /// <returns>租户版本列表项 DTO</returns>
    public static TenantEditionListItemDto ToListItemDto(SysTenantEdition edition)
    {
        ArgumentNullException.ThrowIfNull(edition);

        return new TenantEditionListItemDto
        {
            Id = edition.Id,
            EditionCode = edition.EditionCode,
            EditionName = edition.EditionName,
            Description = edition.Description,
            UserLimit = edition.UserLimit,
            StorageLimit = edition.StorageLimit,
            Price = edition.Price,
            BillingPeriodMonths = edition.BillingPeriodMonths,
            IsFree = edition.IsFree,
            IsDefault = edition.IsDefault,
            Status = edition.Status,
            Sort = edition.Sort,
            CreatedAt = edition.CreatedAt,
            ModifiedAt = edition.ModifiedAt
        };
    }

    /// <summary>
    /// 映射租户版本详情
    /// </summary>
    /// <param name="edition">租户版本</param>
    /// <returns>租户版本详情 DTO</returns>
    public static TenantEditionDetailDto ToDetailDto(SysTenantEdition edition)
    {
        ArgumentNullException.ThrowIfNull(edition);

        return new TenantEditionDetailDto
        {
            Id = edition.Id,
            EditionCode = edition.EditionCode,
            EditionName = edition.EditionName,
            Description = edition.Description,
            UserLimit = edition.UserLimit,
            StorageLimit = edition.StorageLimit,
            Price = edition.Price,
            BillingPeriodMonths = edition.BillingPeriodMonths,
            IsFree = edition.IsFree,
            IsDefault = edition.IsDefault,
            Status = edition.Status,
            Sort = edition.Sort,
            Remark = edition.Remark,
            CreatedAt = edition.CreatedAt,
            CreatedId = edition.CreatedId,
            CreatedBy = edition.CreatedBy,
            ModifiedAt = edition.ModifiedAt,
            ModifiedId = edition.ModifiedId,
            ModifiedBy = edition.ModifiedBy
        };
    }
}
