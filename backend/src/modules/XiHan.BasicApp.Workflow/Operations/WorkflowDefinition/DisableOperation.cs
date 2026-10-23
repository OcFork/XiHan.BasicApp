// 由 xihan migrate v4 生成；提交前请检查。

using System.Globalization;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Workflow.Application.AppServices;
using XiHan.BasicApp.Workflow.Application.Business;
using XiHan.BasicApp.Workflow.Application.Contracts;
using XiHan.BasicApp.Workflow.Application.Dtos;
using XiHan.BasicApp.Workflow.Application.Mappers;
using XiHan.BasicApp.Workflow.Domain.Entities;
using XiHan.BasicApp.Workflow.Domain.Permissions;
using XiHan.BasicApp.Workflow.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Workflow.Operations.WorkflowDefinition;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("workflow-definition.disable", Permission = WorkflowPermissionCodes.Update)]
public sealed class DisableOperation(IWorkflowDefinitionAppService service) : IOperation<WorkflowDefinitionIdDto, WorkflowDefinitionDetailDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<WorkflowDefinitionDetailDto>> HandleAsync(WorkflowDefinitionIdDto request, InvocationContext context)
        => await service.DisableAsync(request, context.CancellationToken).ConfigureAwait(false);
}
