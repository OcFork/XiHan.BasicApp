// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Repositories;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.ImportHistory;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("import-history.create", Policy = "authenticated")]
public sealed class CreateOperation(IImportHistoryAppService service) : IOperation<ImportHistoryCreateDto, ImportHistoryDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<ImportHistoryDto>> HandleAsync(ImportHistoryCreateDto request, InvocationContext context)
        => await service.CreateAsync(request, context.CancellationToken).ConfigureAwait(false);
}
