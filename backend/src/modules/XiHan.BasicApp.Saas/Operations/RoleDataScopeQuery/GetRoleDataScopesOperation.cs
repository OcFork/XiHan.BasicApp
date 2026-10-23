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

namespace XiHan.BasicApp.Saas.Operations.RoleDataScopeQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("role-data-scope-query.get-role-data-scopes", Permission = SaasPermissionCodes.RoleDataScope.Read)]
public sealed class GetRoleDataScopesOperation(IRoleDataScopeQueryService service) : IOperation<GetRoleDataScopesRequest, IReadOnlyList<RoleDataScopeListItemDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<IReadOnlyList<RoleDataScopeListItemDto>>> HandleAsync(GetRoleDataScopesRequest request, InvocationContext context)
        => await service.GetRoleDataScopesAsync(request.RoleId, request.OnlyValid, context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetRoleDataScopesRequest(long RoleId, bool OnlyValid = false);
