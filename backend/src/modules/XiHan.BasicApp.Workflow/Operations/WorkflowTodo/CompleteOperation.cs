// 由 xihan migrate v4 生成；提交前请检查。

using System.Globalization;
using System.Text.Json;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Enums;
using XiHan.BasicApp.Workflow.Application.AppServices;
using XiHan.BasicApp.Workflow.Application.Business;
using XiHan.BasicApp.Workflow.Application.Contracts;
using XiHan.BasicApp.Workflow.Application.Dtos;
using XiHan.BasicApp.Workflow.Domain.Entities;
using XiHan.BasicApp.Workflow.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Workflow.Operations.WorkflowTodo;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("workflow-todo.complete", Policy = "authenticated")]
public sealed class CompleteOperation(IWorkflowTodoAppService service) : IOperation<WorkflowTodoCompleteDto, WorkflowTodoCompleteResultDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<WorkflowTodoCompleteResultDto>> HandleAsync(WorkflowTodoCompleteDto request, InvocationContext context)
        => await service.CompleteAsync(request, context.CancellationToken).ConfigureAwait(false);
}
