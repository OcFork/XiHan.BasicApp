// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using XiHan.BasicApp.CodeGeneration.Application.AppServices;
using XiHan.BasicApp.CodeGeneration.Application.Contracts;
using XiHan.BasicApp.CodeGeneration.Application.Dtos;
using XiHan.BasicApp.CodeGeneration.Domain.Entities;
using XiHan.BasicApp.CodeGeneration.Domain.Enums;
using XiHan.BasicApp.CodeGeneration.Domain.Generation;
using XiHan.BasicApp.CodeGeneration.Domain.Permissions;
using XiHan.BasicApp.CodeGeneration.Domain.Repositories;
using XiHan.BasicApp.CodeGeneration.Operations;
using XiHan.BasicApp.Saas.Domain.Enums;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Localization.Abstractions;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.CodeGeneration.Operations.CodeGeneration;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("code-generation.preview", Permission = CodeGenPermissionCodes.Read)]
public sealed class PreviewOperation(ICodeGenerationAppService service) : IOperation<CodeGenPreviewRequestDto, CodeGenResultDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<CodeGenResultDto>> HandleAsync(CodeGenPreviewRequestDto request, InvocationContext context)
        => await service.PreviewAsync(request, context.CancellationToken).ConfigureAwait(false);
}
