// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.QueryServices;
using XiHan.BasicApp.Saas.Domain.Repositories;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.ImportHistoryQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("import-history-query.get-mine", Policy = "authenticated")]
public sealed class GetMineOperation(IImportHistoryQueryService service) : IOperation<GetMineRequest, List<ImportHistoryDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<List<ImportHistoryDto>>> HandleAsync(GetMineRequest request, InvocationContext context)
        => await service.GetMineAsync(request.PageCode, request.Count, context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetMineRequest(string PageCode, int Count = 10);
