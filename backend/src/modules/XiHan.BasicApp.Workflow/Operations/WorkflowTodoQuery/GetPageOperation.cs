// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using XiHan.BasicApp.Workflow.Application.Business;
using XiHan.BasicApp.Workflow.Application.Contracts;
using XiHan.BasicApp.Workflow.Application.Dtos;
using XiHan.BasicApp.Workflow.Application.Mappers;
using XiHan.BasicApp.Workflow.Application.QueryServices;
using XiHan.BasicApp.Workflow.Infrastructure.Stores;
using XiHan.BasicApp.Workflow.Operations;
using XiHan.Framework.Domain.Shared.Paging.Dtos;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Workflow.Operations.WorkflowTodoQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("workflow-todo-query.list", Policy = "authenticated")]
public sealed class GetPageOperation(IWorkflowTodoQueryService service) : IOperation<WorkflowTodoPageQueryDto, PageResultDtoBase<WorkflowTodoListItemDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<PageResultDtoBase<WorkflowTodoListItemDto>>> HandleAsync(WorkflowTodoPageQueryDto request, InvocationContext context)
        => await service.GetPageAsync(request, context.CancellationToken).ConfigureAwait(false);
}
