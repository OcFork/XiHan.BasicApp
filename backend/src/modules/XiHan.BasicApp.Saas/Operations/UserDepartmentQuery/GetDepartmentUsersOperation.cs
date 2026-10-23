// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Application.QueryServices;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Enums;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Domain.Repositories;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.UserDepartmentQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("user-department-query.get-department-users", Permission = SaasPermissionCodes.UserDepartment.Read)]
public sealed class GetDepartmentUsersOperation(IUserDepartmentQueryService service) : IOperation<GetDepartmentUsersRequest, IReadOnlyList<UserDepartmentListItemDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<IReadOnlyList<UserDepartmentListItemDto>>> HandleAsync(GetDepartmentUsersRequest request, InvocationContext context)
        => await service.GetDepartmentUsersAsync(request.DepartmentId, request.IncludeChildren, request.OnlyValid, context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetDepartmentUsersRequest(long DepartmentId, bool IncludeChildren = false, bool OnlyValid = false);
