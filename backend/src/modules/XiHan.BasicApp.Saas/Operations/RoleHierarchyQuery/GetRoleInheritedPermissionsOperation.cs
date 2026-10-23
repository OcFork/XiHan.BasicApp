// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.QueryServices;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Domain.Repositories;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.RoleHierarchyQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("role-hierarchy-query.get-role-inherited-permissions", Permission = SaasPermissionCodes.RolePermission.Read)]
public sealed class GetRoleInheritedPermissionsOperation(IRoleHierarchyQueryService service) : IOperation<GetRoleInheritedPermissionsRequest, IReadOnlyList<RoleInheritedPermissionDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<IReadOnlyList<RoleInheritedPermissionDto>>> HandleAsync(GetRoleInheritedPermissionsRequest request, InvocationContext context)
        => await service.GetRoleInheritedPermissionsAsync(request.RoleId, context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetRoleInheritedPermissionsRequest(long RoleId);
