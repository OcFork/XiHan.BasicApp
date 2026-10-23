// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Caching;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.Menu;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("menu.update-status", Permission = SaasPermissionCodes.Menu.Status)]
public sealed class UpdateMenuStatusOperation(IMenuAppService service) : IOperation<MenuStatusUpdateDto, MenuDetailDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<MenuDetailDto>> HandleAsync(MenuStatusUpdateDto request, InvocationContext context)
        => await service.UpdateMenuStatusAsync(request, context.CancellationToken).ConfigureAwait(false);
}
