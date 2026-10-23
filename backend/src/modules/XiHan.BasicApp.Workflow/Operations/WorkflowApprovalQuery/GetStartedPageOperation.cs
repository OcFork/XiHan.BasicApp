// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Linq.Expressions;
using System.Text.Json;
using XiHan.BasicApp.Core.Dtos;
using XiHan.BasicApp.Saas.Application.Extensions;
using XiHan.BasicApp.Workflow.Application.Business;
using XiHan.BasicApp.Workflow.Application.Contracts;
using XiHan.BasicApp.Workflow.Application.Dtos;
using XiHan.BasicApp.Workflow.Application.Mappers;
using XiHan.BasicApp.Workflow.Application.QueryServices;
using XiHan.BasicApp.Workflow.Domain.Entities;
using XiHan.BasicApp.Workflow.Domain.Permissions;
using XiHan.BasicApp.Workflow.Domain.Repositories;
using XiHan.BasicApp.Workflow.Infrastructure.Stores;
using XiHan.BasicApp.Workflow.Operations;
using XiHan.Framework.Domain.Shared.Paging.Dtos;
using XiHan.Framework.Domain.Shared.Paging.Enums;
using XiHan.Framework.Domain.Shared.Paging.Models;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Workflow.Operations.WorkflowApprovalQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("workflow-approval-query.list-started", Policy = "authenticated")]
public sealed class GetStartedPageOperation(IWorkflowApprovalQueryService service) : IOperation<WorkflowApprovalPageQueryDto, PageResultDtoBase<WorkflowApprovalInstanceItemDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<PageResultDtoBase<WorkflowApprovalInstanceItemDto>>> HandleAsync(WorkflowApprovalPageQueryDto request, InvocationContext context)
        => await service.GetStartedPageAsync(request, context.CancellationToken).ConfigureAwait(false);
}
