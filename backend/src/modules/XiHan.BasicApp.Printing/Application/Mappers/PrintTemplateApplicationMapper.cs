// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Globalization;
using XiHan.BasicApp.Printing.Application.Dtos;
using XiHan.BasicApp.Printing.Domain.DomainServices;
using XiHan.BasicApp.Printing.Domain.Entities;
using XiHan.BasicApp.Core.Exceptions;

namespace XiHan.BasicApp.Printing.Application.Mappers;

/// <summary>
/// 打印模板 DTO、领域命令和实体之间的纯映射器。
/// </summary>
public static class PrintTemplateApplicationMapper
{
    /// <summary>
    /// 把创建 DTO 映射为领域命令。
    /// </summary>
    /// <param name="input">创建 DTO。</param>
    /// <returns>创建命令。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="input"/> 为空。</exception>
    public static PrintTemplateCreateCommand ToCreateCommand(PrintTemplateCreateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);
        return new PrintTemplateCreateCommand(
            input.TemplateCode,
            input.DataSourceCode,
            input.TemplateName,
            input.TemplateJson,
            input.AllowTenantUse,
            input.Status,
            input.Sort,
            input.Remark);
    }

    /// <summary>
    /// 把更新 DTO 映射为包含乐观并发版本的领域命令。
    /// </summary>
    /// <param name="input">更新 DTO。</param>
    /// <returns>更新命令。</returns>
    /// <exception cref="UserFriendlyException">行版本不是非负十进制整数。</exception>
    public static PrintTemplateUpdateCommand ToUpdateCommand(PrintTemplateUpdateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);
        return new PrintTemplateUpdateCommand(
            input.Id,
            ParseRowVersion(input.ConcurrencyStamp),
            input.DataSourceCode,
            input.TemplateName,
            input.TemplateJson,
            input.AllowTenantUse,
            input.Sort,
            input.Remark);
    }

    /// <summary>
    /// 把状态 DTO 映射为领域命令。
    /// </summary>
    /// <param name="input">状态 DTO。</param>
    /// <returns>状态变更命令。</returns>
    /// <exception cref="UserFriendlyException">行版本无效。</exception>
    public static PrintTemplateStatusChangeCommand ToStatusCommand(PrintTemplateStatusUpdateDto input)
    {
        ArgumentNullException.ThrowIfNull(input);
        return new PrintTemplateStatusChangeCommand(
            input.Id,
            ParseRowVersion(input.ConcurrencyStamp),
            input.Status,
            input.Remark);
    }

    /// <summary>
    /// 把删除 DTO 映射为领域命令。
    /// </summary>
    /// <param name="input">删除 DTO。</param>
    /// <returns>删除命令。</returns>
    /// <exception cref="UserFriendlyException">行版本无效。</exception>
    public static PrintTemplateDeleteCommand ToDeleteCommand(PrintTemplateDeleteDto input)
    {
        ArgumentNullException.ThrowIfNull(input);
        return new PrintTemplateDeleteCommand(input.Id, ParseRowVersion(input.ConcurrencyStamp));
    }

    /// <summary>
    /// 把实体映射为列表项，行版本始终按十进制字符串输出。
    /// </summary>
    /// <param name="template">模板实体。</param>
    /// <returns>列表项 DTO。</returns>
    public static PrintTemplateListItemDto ToListItemDto(SysPrintTemplate template)
    {
        ArgumentNullException.ThrowIfNull(template);
        return new PrintTemplateListItemDto
        {
            Id = template.Id,
            TemplateCode = template.TemplateCode,
            DataSourceCode = template.DataSourceCode,
            TemplateName = template.TemplateName,
            IsGlobal = template.IsGlobal,
            AllowTenantUse = template.AllowTenantUse,
            Status = template.Status,
            Sort = template.Sort,
            Remark = template.Remark,
            ConcurrencyStamp = template.ConcurrencyStamp.ToString(CultureInfo.InvariantCulture)
        };
    }

    /// <summary>
    /// 把实体映射为包含模板 JSON 和审计时间的详情 DTO。
    /// </summary>
    /// <param name="template">模板实体。</param>
    /// <returns>详情 DTO。</returns>
    public static PrintTemplateDetailDto ToDetailDto(SysPrintTemplate template)
    {
        ArgumentNullException.ThrowIfNull(template);
        var item = ToListItemDto(template);
        return new PrintTemplateDetailDto
        {
            Id = item.Id,
            TemplateCode = item.TemplateCode,
            DataSourceCode = item.DataSourceCode,
            TemplateName = item.TemplateName,
            TemplateJson = template.TemplateJson,
            IsGlobal = item.IsGlobal,
            AllowTenantUse = item.AllowTenantUse,
            Status = item.Status,
            Sort = item.Sort,
            Remark = item.Remark,
            ConcurrencyStamp = item.ConcurrencyStamp,
            CreatedAt = template.CreatedAt,
            ModifiedAt = template.ModifiedAt
        };
    }

    /// <summary>
    /// 规范化客户端带来的并发戳：空白视为未提供
    /// </summary>
    public static string? ParseRowVersion(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
