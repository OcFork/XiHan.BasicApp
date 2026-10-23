// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.Conditions;
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

namespace XiHan.BasicApp.Saas.Operations.UserPermissionQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("user-permission-query.get-user-permission-detail", Permission = SaasPermissionCodes.UserPermission.Read)]
public sealed class GetUserPermissionDetailOperation(IUserPermissionQueryService service) : IOperation<GetUserPermissionDetailRequest, UserPermissionDetailDto?>
{
    /// <inheritdoc />
    public async ValueTask<Result<UserPermissionDetailDto?>> HandleAsync(GetUserPermissionDetailRequest request, InvocationContext context)
        => await service.GetUserPermissionDetailAsync(request.Id, context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetUserPermissionDetailRequest(long Id);
