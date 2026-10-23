// 由 xihan migrate v4 生成；提交前请检查。

using XiHan.BasicApp.CodeGeneration.Application.AppServices;
using XiHan.BasicApp.CodeGeneration.Application.Contracts;
using XiHan.BasicApp.CodeGeneration.Application.Dtos;
using XiHan.BasicApp.CodeGeneration.Application.Mappers;
using XiHan.BasicApp.CodeGeneration.Domain.DomainServices;
using XiHan.BasicApp.CodeGeneration.Domain.Permissions;
using XiHan.BasicApp.CodeGeneration.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.CodeGeneration.Operations.CodeGenTableColumn;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("code-gen-table-column.update", Permission = CodeGenPermissionCodes.Update)]
public sealed class UpdateOperation(ICodeGenTableColumnAppService service) : IOperation<CodeGenTableColumnUpdateDto, CodeGenTableColumnListItemDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<CodeGenTableColumnListItemDto>> HandleAsync(CodeGenTableColumnUpdateDto request, InvocationContext context)
        => await service.UpdateAsync(request, context.CancellationToken).ConfigureAwait(false);
}
