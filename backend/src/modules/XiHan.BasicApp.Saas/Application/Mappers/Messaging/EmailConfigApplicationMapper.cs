// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Entities;

namespace XiHan.BasicApp.Saas.Application.Mappers;

/// <summary>
/// 邮件配置应用层映射器
/// </summary>
public static class EmailConfigApplicationMapper
{
    /// <summary>
    /// 映射邮件配置创建命令
    /// </summary>
    public static EmailConfigCreateCommand ToCreateCommand(EmailConfigCreateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return new EmailConfigCreateCommand(
            input.ConfigCode,
            input.ConfigName,
            input.SmtpHost,
            input.SmtpPort,
            input.UseSsl,
            input.AcceptInvalidCertificate,
            input.FromEmail,
            input.FromName,
            input.UserName,
            input.Password,
            input.IsBodyHtml,
            input.IsDefault,
            input.IsEnabled,
            input.Sort,
            input.Remark);
    }

    /// <summary>
    /// 映射邮件配置更新命令
    /// </summary>
    public static EmailConfigUpdateCommand ToUpdateCommand(EmailConfigUpdateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return new EmailConfigUpdateCommand(
            input.Id,
            input.ConfigName,
            input.SmtpHost,
            input.SmtpPort,
            input.UseSsl,
            input.AcceptInvalidCertificate,
            input.FromEmail,
            input.FromName,
            input.UserName,
            input.Password,
            input.IsBodyHtml,
            input.Sort,
            input.Remark);
    }

    /// <summary>
    /// 映射邮件配置状态命令
    /// </summary>
    public static EmailConfigStatusChangeCommand ToStatusCommand(EmailConfigStatusUpdateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return new EmailConfigStatusChangeCommand(input.Id, input.IsEnabled);
    }

    /// <summary>
    /// 映射邮件配置默认变更命令
    /// </summary>
    public static EmailConfigDefaultChangeCommand ToDefaultCommand(EmailConfigDefaultUpdateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return new EmailConfigDefaultChangeCommand(input.Id);
    }

    /// <summary>
    /// 映射邮件配置列表项（Password 脱敏，仅标识是否已配置）
    /// </summary>
    /// <param name="config">邮件配置</param>
    /// <returns>邮件配置列表项 DTO</returns>
    public static EmailConfigListItemDto ToListItemDto(SysEmailConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);

        return new EmailConfigListItemDto
        {
            Id = config.Id,
            ConfigCode = config.ConfigCode,
            ConfigName = config.ConfigName,
            SmtpHost = config.SmtpHost,
            SmtpPort = config.SmtpPort,
            UseSsl = config.UseSsl,
            FromEmail = config.FromEmail,
            FromName = config.FromName,
            HasPassword = !string.IsNullOrEmpty(config.Password),
            IsDefault = config.IsDefault,
            IsEnabled = config.IsEnabled,
            Sort = config.Sort,
            CreatedAt = config.CreatedAt,
            ModifiedAt = config.ModifiedAt
        };
    }

    /// <summary>
    /// 映射邮件配置详情（Password 脱敏，仅标识是否已配置）
    /// </summary>
    /// <param name="config">邮件配置</param>
    /// <returns>邮件配置详情 DTO</returns>
    public static EmailConfigDetailDto ToDetailDto(SysEmailConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);

        return new EmailConfigDetailDto
        {
            Id = config.Id,
            ConfigCode = config.ConfigCode,
            ConfigName = config.ConfigName,
            SmtpHost = config.SmtpHost,
            SmtpPort = config.SmtpPort,
            UseSsl = config.UseSsl,
            AcceptInvalidCertificate = config.AcceptInvalidCertificate,
            FromEmail = config.FromEmail,
            FromName = config.FromName,
            UserName = config.UserName,
            HasPassword = !string.IsNullOrEmpty(config.Password),
            IsBodyHtml = config.IsBodyHtml,
            IsDefault = config.IsDefault,
            IsEnabled = config.IsEnabled,
            Sort = config.Sort,
            Remark = config.Remark,
            CreatedAt = config.CreatedAt,
            CreatedId = config.CreatedId,
            CreatedBy = config.CreatedBy,
            ModifiedAt = config.ModifiedAt,
            ModifiedId = config.ModifiedId,
            ModifiedBy = config.ModifiedBy
        };
    }
}
