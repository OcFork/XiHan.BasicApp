// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.Linq.Expressions;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Application.QueryServices;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Domain.Shared.Paging.Dtos;
using XiHan.Framework.Domain.Shared.Paging.Models;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.OperationLogQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("operation-log-query.list-operation-log", Permission = SaasPermissionCodes.OperationLog.Read)]
public sealed class GetOperationLogPageOperation(IOperationLogQueryService service) : IOperation<OperationLogPageQueryDto, PageResultDtoBase<OperationLogListItemDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<PageResultDtoBase<OperationLogListItemDto>>> HandleAsync(OperationLogPageQueryDto request, InvocationContext context)
        => await service.GetOperationLogPageAsync(request, context.CancellationToken).ConfigureAwait(false);
}
