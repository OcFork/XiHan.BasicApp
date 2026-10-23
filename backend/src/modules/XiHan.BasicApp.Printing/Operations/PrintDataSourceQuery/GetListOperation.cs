// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Printing.Application.Contracts;
using XiHan.BasicApp.Printing.Application.Dtos;
using XiHan.BasicApp.Printing.Application.QueryServices;
using XiHan.BasicApp.Printing.Domain.DataSources;
using XiHan.BasicApp.Printing.Domain.Permissions;
using XiHan.BasicApp.Printing.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Printing.Operations.PrintDataSourceQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("print-data-source-query.list", Policy = "authenticated")]
public sealed class GetListOperation(IPrintDataSourceQueryService service) : IOperation<GetListRequest, List<PrintDataSourceDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<List<PrintDataSourceDto>>> HandleAsync(GetListRequest request, InvocationContext context)
        => await service.GetListAsync(context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetListRequest();
