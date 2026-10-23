// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.AppServices;
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

namespace XiHan.BasicApp.Saas.Operations.StorageConfig;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("storage-config.update-status", Permission = SaasPermissionCodes.StorageConfig.Status)]
public sealed class UpdateStorageConfigStatusOperation(IStorageConfigAppService service) : IOperation<StorageConfigStatusUpdateDto, StorageConfigDetailDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<StorageConfigDetailDto>> HandleAsync(StorageConfigStatusUpdateDto request, InvocationContext context)
        => await service.UpdateStorageConfigStatusAsync(request, context.CancellationToken).ConfigureAwait(false);
}
