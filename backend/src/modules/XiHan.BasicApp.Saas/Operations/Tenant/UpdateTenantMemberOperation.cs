// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Caching;
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
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.Tenant;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("tenant.update-member", Permission = SaasPermissionCodes.TenantMember.Update)]
public sealed class UpdateTenantMemberOperation(ITenantAppService service) : IOperation<TenantMemberUpdateDto, TenantMemberDetailDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<TenantMemberDetailDto>> HandleAsync(TenantMemberUpdateDto request, InvocationContext context)
        => await service.UpdateTenantMemberAsync(request, context.CancellationToken).ConfigureAwait(false);
}
