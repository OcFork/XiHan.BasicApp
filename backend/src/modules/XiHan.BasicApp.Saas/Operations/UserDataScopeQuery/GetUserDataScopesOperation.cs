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

namespace XiHan.BasicApp.Saas.Operations.UserDataScopeQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("user-data-scope-query.get-user-data-scopes", Permission = SaasPermissionCodes.UserDataScope.Read)]
public sealed class GetUserDataScopesOperation(IUserDataScopeQueryService service) : IOperation<GetUserDataScopesRequest, IReadOnlyList<UserDataScopeListItemDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<IReadOnlyList<UserDataScopeListItemDto>>> HandleAsync(GetUserDataScopesRequest request, InvocationContext context)
        => await service.GetUserDataScopesAsync(request.UserId, request.OnlyValid, context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetUserDataScopesRequest(long UserId, bool OnlyValid = false);
