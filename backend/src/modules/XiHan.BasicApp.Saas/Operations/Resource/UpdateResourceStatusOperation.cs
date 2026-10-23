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

namespace XiHan.BasicApp.Saas.Operations.Resource;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("resource.update-status", Permission = SaasPermissionCodes.Resource.Status)]
public sealed class UpdateResourceStatusOperation(IResourceAppService service) : IOperation<ResourceStatusUpdateDto, ResourceDetailDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<ResourceDetailDto>> HandleAsync(ResourceStatusUpdateDto request, InvocationContext context)
        => await service.UpdateResourceStatusAsync(request, context.CancellationToken).ConfigureAwait(false);
}
