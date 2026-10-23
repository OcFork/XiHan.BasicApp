// 由 xihan migrate v4 生成；提交前请检查。

using System.Globalization;
using XiHan.BasicApp.Workflow.Application.AppServices;
using XiHan.BasicApp.Workflow.Application.Business;
using XiHan.BasicApp.Workflow.Application.Contracts;
using XiHan.BasicApp.Workflow.Application.Dtos;
using XiHan.BasicApp.Workflow.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Workflow.Operations.WorkflowApproval;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("workflow-approval.withdraw", Policy = "authenticated")]
public sealed class WithdrawOperation(IWorkflowApprovalAppService service) : IOperation<WorkflowApprovalWithdrawDto, EmptyResponse>
{
    /// <inheritdoc />
    public async ValueTask<Result<EmptyResponse>> HandleAsync(WorkflowApprovalWithdrawDto request, InvocationContext context)
    {
        await service.WithdrawAsync(request, context.CancellationToken).ConfigureAwait(false);
        return EmptyResponse.Instance;
    }
}
