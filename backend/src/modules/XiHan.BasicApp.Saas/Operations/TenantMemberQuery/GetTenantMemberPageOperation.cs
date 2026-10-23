// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XiHan.BasicApp.Core.Dtos;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Extensions;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Application.QueryServices;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Domain.Repositories;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Domain.Shared.Paging.Dtos;
using XiHan.Framework.Domain.Shared.Paging.Enums;
using XiHan.Framework.Domain.Shared.Paging.Models;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.MultiTenancy.Abstractions;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.TenantMemberQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("tenant-member-query.list-tenant-member", Permission = SaasPermissionCodes.TenantMember.Read)]
public sealed class GetTenantMemberPageOperation(ITenantMemberQueryService service) : IOperation<TenantMemberPageQueryDto, PageResultDtoBase<TenantMemberListItemDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<PageResultDtoBase<TenantMemberListItemDto>>> HandleAsync(TenantMemberPageQueryDto request, InvocationContext context)
        => await service.GetTenantMemberPageAsync(request, context.CancellationToken).ConfigureAwait(false);
}
