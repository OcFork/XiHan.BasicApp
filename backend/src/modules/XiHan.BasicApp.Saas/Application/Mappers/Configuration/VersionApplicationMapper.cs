// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Entities;

namespace XiHan.BasicApp.Saas.Application.Mappers;

/// <summary>
/// 系统版本应用层映射器
/// </summary>
public static class VersionApplicationMapper
{
    /// <summary>
    /// 映射系统版本创建命令
    /// </summary>
    public static VersionCreateCommand ToCreateCommand(VersionCreateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return new VersionCreateCommand(
            input.AppVersion,
            input.DbVersion,
            input.MinSupportVersion,
            input.IsUpgrading,
            input.UpgradeNode,
            input.UpgradeStartTime);
    }

    /// <summary>
    /// 映射系统版本更新命令
    /// </summary>
    public static VersionUpdateCommand ToUpdateCommand(VersionUpdateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return new VersionUpdateCommand(
            input.Id,
            input.AppVersion,
            input.DbVersion,
            input.MinSupportVersion,
            input.IsUpgrading,
            input.UpgradeNode,
            input.UpgradeStartTime);
    }

    /// <summary>
    /// 映射系统升级开始命令
    /// </summary>
    public static VersionUpgradeStartCommand ToUpgradeStartCommand(VersionUpgradeStartDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return new VersionUpgradeStartCommand(input.Id, input.UpgradeNode, input.UpgradeStartTime);
    }

    /// <summary>
    /// 映射系统升级完成命令
    /// </summary>
    public static VersionUpgradeFinishCommand ToUpgradeFinishCommand(VersionUpgradeFinishDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return new VersionUpgradeFinishCommand(input.Id, input.AppVersion, input.DbVersion, input.MinSupportVersion);
    }

    /// <summary>
    /// 映射系统版本列表项
    /// </summary>
    /// <param name="version">系统版本实体</param>
    /// <returns>系统版本列表项 DTO</returns>
    public static VersionListItemDto ToListItemDto(SysVersion version)
    {
        ArgumentNullException.ThrowIfNull(version);

        return new VersionListItemDto
        {
            Id = version.Id,
            AppVersion = version.AppVersion,
            DbVersion = version.DbVersion,
            MinSupportVersion = version.MinSupportVersion,
            IsUpgrading = version.IsUpgrading,
            UpgradeNode = version.UpgradeNode,
            UpgradeStartTime = version.UpgradeStartTime,
            CreatedAt = version.CreatedAt
        };
    }

    /// <summary>
    /// 映射系统版本详情
    /// </summary>
    /// <param name="version">系统版本实体</param>
    /// <returns>系统版本详情 DTO</returns>
    public static VersionDetailDto ToDetailDto(SysVersion version)
    {
        ArgumentNullException.ThrowIfNull(version);

        var item = ToListItemDto(version);
        return new VersionDetailDto
        {
            Id = item.Id,
            AppVersion = item.AppVersion,
            DbVersion = item.DbVersion,
            MinSupportVersion = item.MinSupportVersion,
            IsUpgrading = item.IsUpgrading,
            UpgradeNode = item.UpgradeNode,
            UpgradeStartTime = item.UpgradeStartTime,
            CreatedAt = item.CreatedAt,
            CreatedId = version.CreatedId,
            CreatedBy = version.CreatedBy
        };
    }

    /// <summary>
    /// 映射系统迁移历史列表项
    /// </summary>
    /// <param name="migrationHistory">系统迁移历史实体</param>
    /// <returns>系统迁移历史列表项 DTO</returns>
    public static MigrationHistoryListItemDto ToMigrationHistoryListItemDto(SysMigrationHistory migrationHistory)
    {
        ArgumentNullException.ThrowIfNull(migrationHistory);

        return new MigrationHistoryListItemDto
        {
            Id = migrationHistory.Id,
            Version = migrationHistory.Version,
            ScriptName = migrationHistory.ScriptName,
            ExecutedTime = migrationHistory.ExecutedTime,
            Success = migrationHistory.Success,
            NodeName = migrationHistory.NodeName,
            CreatedAt = migrationHistory.CreatedAt
        };
    }

    /// <summary>
    /// 映射系统迁移历史详情
    /// </summary>
    /// <param name="migrationHistory">系统迁移历史实体</param>
    /// <returns>系统迁移历史详情 DTO</returns>
    public static MigrationHistoryDetailDto ToMigrationHistoryDetailDto(SysMigrationHistory migrationHistory)
    {
        ArgumentNullException.ThrowIfNull(migrationHistory);

        var item = ToMigrationHistoryListItemDto(migrationHistory);
        return new MigrationHistoryDetailDto
        {
            Id = item.Id,
            Version = item.Version,
            ScriptName = item.ScriptName,
            ExecutedTime = item.ExecutedTime,
            Success = item.Success,
            NodeName = item.NodeName,
            CreatedAt = item.CreatedAt,
            ErrorMessage = migrationHistory.ErrorMessage,
            CreatedId = migrationHistory.CreatedId,
            CreatedBy = migrationHistory.CreatedBy
        };
    }
}
