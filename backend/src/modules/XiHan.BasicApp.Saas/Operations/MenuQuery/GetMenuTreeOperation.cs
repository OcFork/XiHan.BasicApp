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
using XiHan.BasicApp.Saas.Domain.Enums;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Domain.Repositories;
using XiHan.BasicApp.Saas.Operations;
using XiHan.BasicApp.Core.Paging.Dtos;
using XiHan.BasicApp.Core.Paging.Enums;
using XiHan.BasicApp.Core.Paging.Models;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.MenuQuery;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("menu-query.get-menu-tree", Permission = SaasPermissionCodes.Menu.Read)]
public sealed class GetMenuTreeOperation(IMenuQueryService service) : IOperation<MenuTreeQueryDto, IReadOnlyList<MenuTreeNodeDto>>
{
    /// <inheritdoc />
    public async ValueTask<Result<IReadOnlyList<MenuTreeNodeDto>>> HandleAsync(MenuTreeQueryDto request, InvocationContext context)
        => Result.Success(await service.GetMenuTreeAsync(request, context.CancellationToken).ConfigureAwait(false));
}
