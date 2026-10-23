// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.AspNetCore.Authorization;
using XiHan.BasicApp.Saas.Application.AppServices;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Application.QueryServices;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Events;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Operations;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Operations.File;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
[Operation("file.verify-storage", Permission = SaasPermissionCodes.File.Status)]
public sealed class VerifyFileStorageOperation(IFileAppService service) : IOperation<FileStorageVerifyDto, FileStorageDetailDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<FileStorageDetailDto>> HandleAsync(FileStorageVerifyDto request, InvocationContext context)
        => await service.VerifyFileStorageAsync(request, context.CancellationToken).ConfigureAwait(false);
}
