// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.Linq.Expressions;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Application.QueryServices;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Operations;
using XiHan.BasicApp.Core.Paging.Dtos;
using XiHan.BasicApp.Core.Paging.Models;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.PermissionChangeLogQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("permission-change-log-query.get-permission-change-log-detail", Permission = SaasPermissionCodes.PermissionChangeLog.Read)]
public sealed class GetPermissionChangeLogDetailOperation(IPermissionChangeLogQueryService service) : IOperation<GetPermissionChangeLogDetailRequest, PermissionChangeLogDetailDto?>
{
    /// <inheritdoc />
    public async ValueTask<Result<PermissionChangeLogDetailDto?>> HandleAsync(GetPermissionChangeLogDetailRequest request, InvocationContext context)
        => await service.GetPermissionChangeLogDetailAsync(request.Id, context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetPermissionChangeLogDetailRequest(long Id);
