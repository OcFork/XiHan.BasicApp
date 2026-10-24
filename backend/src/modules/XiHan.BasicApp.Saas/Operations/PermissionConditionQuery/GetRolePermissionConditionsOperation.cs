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
[Operation("permission-condition-query.get-role-permission-conditions", Permission = SaasPermissionCodes.PermissionCondition.Read)]
public sealed class GetRolePermissionConditionsOperation(IPermissionConditionQueryService service) : IOperation<GetRolePermissionConditionsRequest, IReadOnlyList<PermissionConditionListItemDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<IReadOnlyList<PermissionConditionListItemDto>>> HandleAsync(GetRolePermissionConditionsRequest request, InvocationContext context)
        => Result.Success(await service.GetRolePermissionConditionsAsync(request.RolePermissionId, request.OnlyValid, context.CancellationToken).ConfigureAwait(false));
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetRolePermissionConditionsRequest(long RolePermissionId, bool OnlyValid = false);
