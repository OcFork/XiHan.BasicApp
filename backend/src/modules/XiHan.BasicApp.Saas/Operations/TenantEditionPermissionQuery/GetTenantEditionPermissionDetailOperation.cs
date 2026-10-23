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

namespace XiHan.BasicApp.Saas.Operations.TenantEditionPermissionQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("tenant-edition-permission-query.get-tenant-edition-permission-detail", Permission = SaasPermissionCodes.TenantEditionPermission.Read)]
public sealed class GetTenantEditionPermissionDetailOperation(ITenantEditionPermissionQueryService service) : IOperation<GetTenantEditionPermissionDetailRequest, TenantEditionPermissionDetailDto?>
{
    /// <inheritdoc />
    public async ValueTask<Result<TenantEditionPermissionDetailDto?>> HandleAsync(GetTenantEditionPermissionDetailRequest request, InvocationContext context)
        => await service.GetTenantEditionPermissionDetailAsync(request.Id, context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetTenantEditionPermissionDetailRequest(long Id);
