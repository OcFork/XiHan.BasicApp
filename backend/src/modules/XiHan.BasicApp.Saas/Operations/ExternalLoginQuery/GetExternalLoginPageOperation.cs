// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XiHan.BasicApp.Core.Dtos;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Extensions;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Application.QueryServices;
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

namespace XiHan.BasicApp.Saas.Operations.ExternalLoginQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("external-login-query.list-external-login", Permission = SaasPermissionCodes.ExternalLogin.Read)]
public sealed class GetExternalLoginPageOperation(IExternalLoginQueryService service) : IOperation<ExternalLoginPageQueryDto, PageResultDtoBase<ExternalLoginListItemDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<PageResultDtoBase<ExternalLoginListItemDto>>> HandleAsync(ExternalLoginPageQueryDto request, InvocationContext context)
        => await service.GetExternalLoginPageAsync(request, context.CancellationToken).ConfigureAwait(false);
}
