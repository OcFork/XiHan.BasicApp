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

namespace XiHan.BasicApp.Saas.Operations.RoleManagementQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("role-management-query.get-role-management-detail", Permission = SaasPermissionCodes.Role.Read)]
public sealed class GetRoleManagementDetailOperation(IRoleManagementQueryService service) : IOperation<GetRoleManagementDetailRequest, RoleManagementDetailDto?>
{
    /// <inheritdoc />
    public async ValueTask<Result<RoleManagementDetailDto?>> HandleAsync(GetRoleManagementDetailRequest request, InvocationContext context)
        => await service.GetRoleManagementDetailAsync(request.RoleId, context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetRoleManagementDetailRequest(long RoleId);
