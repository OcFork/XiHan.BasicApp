// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
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

namespace XiHan.BasicApp.Saas.Operations.Position;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("position.update-status", Permission = SaasPermissionCodes.Position.Status)]
public sealed class UpdatePositionStatusOperation(IPositionAppService service) : IOperation<PositionStatusUpdateDto, PositionDetailDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<PositionDetailDto>> HandleAsync(PositionStatusUpdateDto request, InvocationContext context)
        => await service.UpdatePositionStatusAsync(request, context.CancellationToken).ConfigureAwait(false);
}
