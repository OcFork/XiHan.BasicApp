// 由 xihan migrate v4 生成；提交前请检查。

using XiHan.BasicApp.CodeGeneration.Application.Contracts;
using XiHan.BasicApp.CodeGeneration.Application.Dtos;
using XiHan.BasicApp.CodeGeneration.Application.Mappers;
using XiHan.BasicApp.CodeGeneration.Application.QueryServices;
using XiHan.BasicApp.CodeGeneration.Domain.Permissions;
using XiHan.BasicApp.CodeGeneration.Domain.Repositories;
using XiHan.BasicApp.CodeGeneration.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.CodeGeneration.Operations.CodeGenTableColumnQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("code-gen-table-column-query.get-by-table", Permission = CodeGenPermissionCodes.Read)]
public sealed class GetByTableOperation(ICodeGenTableColumnQueryService service) : IOperation<GetByTableRequest, IReadOnlyList<CodeGenTableColumnListItemDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<IReadOnlyList<CodeGenTableColumnListItemDto>>> HandleAsync(GetByTableRequest request, InvocationContext context)
        => Result.Success(await service.GetByTableAsync(request.TableId, context.CancellationToken).ConfigureAwait(false));
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetByTableRequest(long TableId);
