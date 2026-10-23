// 由 xihan migrate v4 生成；提交前请检查。

using System.Globalization;
using System.Text.Json;
using XiHan.BasicApp.Workflow.Application.AppServices;
using XiHan.BasicApp.Workflow.Application.Business;
using XiHan.BasicApp.Workflow.Application.Contracts;
using XiHan.BasicApp.Workflow.Application.Dtos;
using XiHan.BasicApp.Workflow.Application.Mappers;
using XiHan.BasicApp.Workflow.Domain.Permissions;
using XiHan.BasicApp.Workflow.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Workflow.Operations.WorkflowInstance;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("workflow-instance.start", Permission = WorkflowPermissionCodes.Execute)]
public sealed class StartOperation(IWorkflowInstanceAppService service) : IOperation<WorkflowInstanceStartDto, WorkflowInstanceListItemDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<WorkflowInstanceListItemDto>> HandleAsync(WorkflowInstanceStartDto request, InvocationContext context)
        => await service.StartAsync(request, context.CancellationToken).ConfigureAwait(false);
}
