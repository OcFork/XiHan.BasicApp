// 由 xihan migrate v4 生成；提交前请检查。

using XiHan.BasicApp.CodeGeneration.Application.AppServices;
using XiHan.BasicApp.CodeGeneration.Application.Contracts;
using XiHan.BasicApp.CodeGeneration.Application.Dtos;
using XiHan.BasicApp.CodeGeneration.Application.Mappers;
using XiHan.BasicApp.CodeGeneration.Domain.DomainServices;
using XiHan.BasicApp.CodeGeneration.Domain.Entities;
using XiHan.BasicApp.CodeGeneration.Domain.Permissions;
using XiHan.BasicApp.CodeGeneration.Operations;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.CodeGeneration.Operations.CodeGenDataSource;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("code-gen-data-source.update", Permission = CodeGenPermissionCodes.Update)]
public sealed class UpdateOperation(ICodeGenDataSourceAppService service) : IOperation<CodeGenDataSourceUpdateDto, CodeGenDataSourceDetailDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<CodeGenDataSourceDetailDto>> HandleAsync(CodeGenDataSourceUpdateDto request, InvocationContext context)
        => await service.UpdateAsync(request, context.CancellationToken).ConfigureAwait(false);
}
