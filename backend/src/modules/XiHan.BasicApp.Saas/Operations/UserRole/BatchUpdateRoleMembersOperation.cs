// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Authorization;
using XiHan.BasicApp.Saas.Application.Caching;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Enums;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Domain.Repositories;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.UserRole;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("user-role.batch-update-role-members", Permission = SaasPermissionCodes.UserRole.Grant)]
public sealed class BatchUpdateRoleMembersOperation(IUserRoleAppService service) : IOperation<RoleMemberBatchUpdateDto, EmptyResponse>
{
    /// <inheritdoc />
    public async ValueTask<Result<EmptyResponse>> HandleAsync(RoleMemberBatchUpdateDto request, InvocationContext context)
    {
        await service.BatchUpdateRoleMembersAsync(request, context.CancellationToken).ConfigureAwait(false);
        return EmptyResponse.Instance;
    }
}
