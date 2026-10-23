// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Enums;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Domain.Repositories;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.Task;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("task.update-run-status", Permission = SaasPermissionCodes.Task.RunStatus)]
public sealed class UpdateTaskRunStatusOperation(ITaskAppService service) : IOperation<TaskRunStatusUpdateDto, TaskDetailDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<TaskDetailDto>> HandleAsync(TaskRunStatusUpdateDto request, InvocationContext context)
        => await service.UpdateTaskRunStatusAsync(request, context.CancellationToken).ConfigureAwait(false);
}
