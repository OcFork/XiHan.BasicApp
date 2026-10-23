// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.CodeGeneration.Application.Contracts;
using XiHan.BasicApp.CodeGeneration.Application.Dtos;
using XiHan.BasicApp.CodeGeneration.Application.Mappers;
using XiHan.BasicApp.CodeGeneration.Domain.DomainServices;
using XiHan.BasicApp.CodeGeneration.Domain.Permissions;

namespace XiHan.BasicApp.CodeGeneration.Application.AppServices;

/// <summary>
/// 代码生成列配置命令应用服务
/// </summary>
public sealed class CodeGenTableColumnAppService : CodeGenerationApplicationService, ICodeGenTableColumnAppService
{
    private readonly ICodeGenTableColumnDomainService _columnDomainService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public CodeGenTableColumnAppService(ICodeGenTableColumnDomainService columnDomainService)
    {
        _columnDomainService = columnDomainService;
    }

    /// <summary>
    /// 按表整体保存列配置（前端表格批量提交）
    /// </summary>
    public async Task BatchSaveAsync(CodeGenTableColumnBatchSaveDto input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);

        var command = CodeGenTableColumnApplicationMapper.ToBatchSaveCommand(input);
        await _columnDomainService.BatchSaveColumnsAsync(command, cancellationToken);
    }

    /// <summary>
    /// 更新单列配置
    /// </summary>
    public async Task<CodeGenTableColumnListItemDto> UpdateAsync(CodeGenTableColumnUpdateDto input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);

        var command = CodeGenTableColumnApplicationMapper.ToUpdateCommand(input);
        var result = await _columnDomainService.UpdateColumnAsync(command, cancellationToken);
        return CodeGenTableColumnApplicationMapper.ToListItemDto(result.Column);
    }
}
