// 由 xihan migrate v4 生成；提交前请检查。

using XiHan.BasicApp.CodeGeneration.Application.AppServices;
using XiHan.BasicApp.CodeGeneration.Application.Contracts;
using XiHan.BasicApp.CodeGeneration.Application.Dtos;
using XiHan.BasicApp.CodeGeneration.Application.Mappers;
using XiHan.BasicApp.CodeGeneration.Domain.DomainServices;
using XiHan.BasicApp.CodeGeneration.Domain.Entities;
using XiHan.BasicApp.CodeGeneration.Domain.Generation;
using XiHan.BasicApp.CodeGeneration.Domain.Permissions;
using XiHan.BasicApp.CodeGeneration.Operations;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.CodeGeneration.Operations.CodeGenTemplate;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("code-gen-template.create", Permission = CodeGenPermissionCodes.Create)]
public sealed class CreateOperation(ICodeGenTemplateAppService service) : IOperation<CodeGenTemplateCreateDto, CodeGenTemplateDetailDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<CodeGenTemplateDetailDto>> HandleAsync(CodeGenTemplateCreateDto request, InvocationContext context)
        => await service.CreateAsync(request, context.CancellationToken).ConfigureAwait(false);
}
