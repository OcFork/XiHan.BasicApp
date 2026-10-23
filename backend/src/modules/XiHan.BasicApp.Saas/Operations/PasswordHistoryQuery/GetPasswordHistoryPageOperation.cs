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
using XiHan.Framework.Domain.Shared.Paging.Dtos;
using XiHan.Framework.Domain.Shared.Paging.Enums;
using XiHan.Framework.Domain.Shared.Paging.Models;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.PasswordHistoryQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("password-history-query.list-password-history", Permission = SaasPermissionCodes.PasswordHistory.Read)]
public sealed class GetPasswordHistoryPageOperation(IPasswordHistoryQueryService service) : IOperation<PasswordHistoryPageQueryDto, PageResultDtoBase<PasswordHistoryListItemDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<PageResultDtoBase<PasswordHistoryListItemDto>>> HandleAsync(PasswordHistoryPageQueryDto request, InvocationContext context)
        => await service.GetPasswordHistoryPageAsync(request, context.CancellationToken).ConfigureAwait(false);
}
