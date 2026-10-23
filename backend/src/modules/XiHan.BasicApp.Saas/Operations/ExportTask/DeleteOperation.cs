// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Exporting;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Repositories;
using XiHan.BasicApp.Saas.Infrastructure.Exporting;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.ExportTask;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("export-task.delete", Policy = "authenticated")]
public sealed class DeleteOperation(IExportTaskAppService service) : IOperation<DeleteRequest, EmptyResponse>
{
    /// <inheritdoc />
    public async ValueTask<Result<EmptyResponse>> HandleAsync(DeleteRequest request, InvocationContext context)
    {
        await service.DeleteAsync(request.Id, context.CancellationToken).ConfigureAwait(false);
        return EmptyResponse.Instance;
    }
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record DeleteRequest(long Id);
