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
[Operation("role-hierarchy-query.get-role-ancestors", Permission = SaasPermissionCodes.RoleHierarchy.Read)]
public sealed class GetRoleAncestorsOperation(IRoleHierarchyQueryService service) : IOperation<GetRoleAncestorsRequest, IReadOnlyList<RoleInheritanceItemDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<IReadOnlyList<RoleInheritanceItemDto>>> HandleAsync(GetRoleAncestorsRequest request, InvocationContext context)
        => Result.Success(await service.GetRoleAncestorsAsync(request.RoleId, context.CancellationToken).ConfigureAwait(false));
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetRoleAncestorsRequest(long RoleId);
