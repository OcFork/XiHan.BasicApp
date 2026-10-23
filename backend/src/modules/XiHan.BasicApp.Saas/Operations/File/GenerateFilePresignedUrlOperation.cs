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
[Operation("file.generate-presigned-url", Permission = SaasPermissionCodes.File.Read)]
public sealed class GenerateFilePresignedUrlOperation(IFileAppService service) : IOperation<GenerateFilePresignedUrlRequest, string>
{
    /// <inheritdoc />
    public async ValueTask<Result<string>> HandleAsync(GenerateFilePresignedUrlRequest request, InvocationContext context)
        => await service.GenerateFilePresignedUrlAsync(request.FileId, request.ExpiresIn, context.CancellationToken).ConfigureAwait(false);
}

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed record GenerateFilePresignedUrlRequest(long FileId, TimeSpan? ExpiresIn = null);
