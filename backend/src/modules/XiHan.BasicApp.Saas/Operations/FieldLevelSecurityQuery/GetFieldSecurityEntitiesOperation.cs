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
using XiHan.BasicApp.Core.Paging.Dtos;
using XiHan.BasicApp.Core.Paging.Enums;
using XiHan.BasicApp.Core.Paging.Models;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.FieldLevelSecurityQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("field-level-security-query.get-field-security-entities", Permission = SaasPermissionCodes.FieldLevelSecurity.Read)]
public sealed class GetFieldSecurityEntitiesOperation(IFieldLevelSecurityQueryService service) : IOperation<GetFieldSecurityEntitiesRequest, IReadOnlyList<FieldSecurityEntityDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<IReadOnlyList<FieldSecurityEntityDto>>> HandleAsync(GetFieldSecurityEntitiesRequest request, InvocationContext context)
        => await service.GetFieldSecurityEntitiesAsync(context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GetFieldSecurityEntitiesRequest();
