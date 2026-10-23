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
[Operation("file.update-metadata", Permission = SaasPermissionCodes.File.Update)]
public sealed class UpdateFileMetadataOperation(IFileAppService service) : IOperation<FileMetadataUpdateDto, FileDetailDto>
{
    /// <inheritdoc />
    public async ValueTask<Result<FileDetailDto>> HandleAsync(FileMetadataUpdateDto request, InvocationContext context)
        => await service.UpdateFileMetadataAsync(request, context.CancellationToken).ConfigureAwait(false);
}
