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
[Operation("task.update-status", Permission = SaasPermissionCodes.Task.Status)]
public sealed class UpdateTaskStatusOperation(ITaskAppService service) : IOperation<TaskStatusUpdateDto, TaskDetailDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<TaskDetailDto>> HandleAsync(TaskStatusUpdateDto request, InvocationContext context)
        => await service.UpdateTaskStatusAsync(request, context.CancellationToken).ConfigureAwait(false);
}
