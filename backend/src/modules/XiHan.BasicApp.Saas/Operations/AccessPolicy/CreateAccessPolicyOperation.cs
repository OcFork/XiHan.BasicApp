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
using XiHan.BasicApp.Core.Security;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.AccessPolicy;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("access-policy.create", Permission = SaasPermissionCodes.AccessPolicy.Create)]
public sealed class CreateAccessPolicyOperation(IAccessPolicyAppService service) : IOperation<AccessPolicyCreateDto, AccessPolicyDetailDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<AccessPolicyDetailDto>> HandleAsync(AccessPolicyCreateDto request, InvocationContext context)
        => await service.CreateAccessPolicyAsync(request, context.CancellationToken).ConfigureAwait(false);
}
