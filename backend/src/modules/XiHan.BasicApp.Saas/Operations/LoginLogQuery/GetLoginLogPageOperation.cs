// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.Linq.Expressions;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Application.QueryServices;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Operations;
using XiHan.BasicApp.Core.Paging.Dtos;
using XiHan.BasicApp.Core.Paging.Models;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.LoginLogQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("login-log-query.list-login-log", Permission = SaasPermissionCodes.LoginLog.Read)]
public sealed class GetLoginLogPageOperation(ILoginLogQueryService service) : IOperation<LoginLogPageQueryDto, PageResultDtoBase<LoginLogListItemDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<PageResultDtoBase<LoginLogListItemDto>>> HandleAsync(LoginLogPageQueryDto request, InvocationContext context)
        => await service.GetLoginLogPageAsync(request, context.CancellationToken).ConfigureAwait(false);
}
