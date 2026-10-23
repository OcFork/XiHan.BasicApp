// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XiHan.BasicApp.Saas.Application.AccessPolicies;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.MultiTenancy.Abstractions;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.AccessPolicy;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("access-policy.unban-login", Permission = SaasPermissionCodes.AccessPolicy.Ban)]
public sealed class UnbanLoginOperation(IAccessPolicyAppService service) : IOperation<LoginUnbanDto, bool>
{
    /// <inheritdoc />
    public async ValueTask<Result<bool>> HandleAsync(LoginUnbanDto request, InvocationContext context)
        => await service.UnbanLoginAsync(request, context.CancellationToken).ConfigureAwait(false);
}
