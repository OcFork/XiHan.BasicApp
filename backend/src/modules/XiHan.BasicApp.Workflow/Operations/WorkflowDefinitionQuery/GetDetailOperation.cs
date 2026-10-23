// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Mvc;
using XiHan.BasicApp.Core.Dtos;
using XiHan.BasicApp.Saas.Application.Extensions;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Workflow.Application.Business;
using XiHan.BasicApp.Workflow.Application.Contracts;
using XiHan.BasicApp.Workflow.Application.Dtos;
using XiHan.BasicApp.Workflow.Application.Mappers;
using XiHan.BasicApp.Workflow.Application.QueryServices;
using XiHan.BasicApp.Workflow.Domain.Entities;
using XiHan.BasicApp.Workflow.Domain.Permissions;
using XiHan.BasicApp.Workflow.Domain.Repositories;
using XiHan.BasicApp.Workflow.Operations;
using XiHan.Framework.Domain.Shared.Paging.Dtos;
using XiHan.Framework.Domain.Shared.Paging.Enums;
using XiHan.Framework.Domain.Shared.Paging.Models;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Workflow.Operations.WorkflowDefinitionQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("workflow-definition-query.get-detail", Permission = WorkflowPermissionCodes.Read)]
public sealed class GetDetailOperation(IWorkflowDefinitionQueryService service) : IOperation<GetDetailRequest, WorkflowDefinitionDetailDto?>
{
    /// <inheritdoc />
    public async ValueTask<Result<WorkflowDefinitionDetailDto?>> HandleAsync(GetDetailRequest request, InvocationContext context)
        => await service.GetDetailAsync(request.Id, context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetDetailRequest(long Id);
