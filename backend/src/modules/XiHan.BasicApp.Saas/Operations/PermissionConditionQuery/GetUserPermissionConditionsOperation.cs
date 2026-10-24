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
[Operation("permission-condition-query.get-user-permission-conditions", Permission = SaasPermissionCodes.PermissionCondition.Read)]
public sealed class GetUserPermissionConditionsOperation(IPermissionConditionQueryService service) : IOperation<GetUserPermissionConditionsRequest, IReadOnlyList<PermissionConditionListItemDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<IReadOnlyList<PermissionConditionListItemDto>>> HandleAsync(GetUserPermissionConditionsRequest request, InvocationContext context)
        => Result.Success(await service.GetUserPermissionConditionsAsync(request.UserPermissionId, request.OnlyValid, context.CancellationToken).ConfigureAwait(false));
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetUserPermissionConditionsRequest(long UserPermissionId, bool OnlyValid = false);
