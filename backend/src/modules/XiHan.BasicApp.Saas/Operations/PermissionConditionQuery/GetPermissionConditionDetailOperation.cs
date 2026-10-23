// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Application.QueryServices;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Domain.Repositories;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.PermissionConditionQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("permission-condition-query.get-permission-condition-detail", Permission = SaasPermissionCodes.PermissionCondition.Read)]
public sealed class GetPermissionConditionDetailOperation(IPermissionConditionQueryService service) : IOperation<GetPermissionConditionDetailRequest, PermissionConditionDetailDto?>
{
    /// <inheritdoc />
    public async ValueTask<Result<PermissionConditionDetailDto?>> HandleAsync(GetPermissionConditionDetailRequest request, InvocationContext context)
        => await service.GetPermissionConditionDetailAsync(request.Id, context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetPermissionConditionDetailRequest(long Id);
