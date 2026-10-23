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
[Operation("export-task.submit", Policy = "authenticated")]
public sealed class SubmitOperation(IExportTaskAppService service) : IOperation<ExportTaskSubmitDto, ExportTaskDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<ExportTaskDto>> HandleAsync(ExportTaskSubmitDto request, InvocationContext context)
        => await service.SubmitAsync(request, context.CancellationToken).ConfigureAwait(false);
}
