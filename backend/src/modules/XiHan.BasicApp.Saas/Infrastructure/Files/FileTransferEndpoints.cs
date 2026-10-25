// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using XiHan.BasicApp.Core.Exceptions;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Operations;
using XiHan.Framework.Hosting.AspNetCore.Endpoints;
using XiHan.Framework.Hosting.AspNetCore;

namespace XiHan.BasicApp.Saas.Infrastructure.Files;

/// <summary>
/// 文件传输端点：操作模型只收 JSON，上传（multipart/form-data）与下载（流）走 ASP.NET Core 端点，
/// 路径沿用 5.0 的集合级动词形态（<c>/api/file:upload</c>、<c>/api/file:download</c>），授权经内核管道按权限码判定
/// </summary>
public static class FileTransferEndpoints
{
    /// <summary>
    /// 表单里文件字段名（与前端 <c>toFileUploadFormData</c> 一致，不分大小写）
    /// </summary>
    private const string FileField = "File";

    /// <summary>
    /// 映射上传与下载端点
    /// </summary>
    /// <param name="endpoints">端点构建器</param>
    /// <param name="routePrefix">与 <c>XiHan:Protocols:Http:RoutePrefix</c> 一致的前缀</param>
    /// <returns>端点构建器</returns>
    public static IEndpointRouteBuilder MapFileTransferEndpoints(this IEndpointRouteBuilder endpoints, string routePrefix = "/api")
    {
        ArgumentNullException.ThrowIfNull(endpoints);
        _ = endpoints.MapPost($"{routePrefix}/file:upload", UploadAsync)
            .DisableAntiforgery()
            .RequireXiHanPermission(SaasPermissionCodes.File.Create);
        _ = endpoints.MapPost($"{routePrefix}/file:download", DownloadAsync)
            .RequireXiHanPermission(SaasPermissionCodes.File.Read);
        _ = endpoints.MapGet($"{routePrefix}/file:download", DownloadAsync)
            .RequireXiHanPermission(SaasPermissionCodes.File.Read);
        return endpoints;
    }

    /// <summary>
    /// 上传：表单字段按 <see cref="FileUploadDto"/> 的属性名读取（不分大小写），文件取 <c>File</c> 字段或第一个文件
    /// </summary>
    private static async Task<IResult> UploadAsync(HttpContext httpContext, IFileAppService service)
    {
        var cancellationToken = httpContext.RequestAborted;
        if (!httpContext.Request.HasFormContentType)
        {
            throw new UserFriendlyException("上传请求必须是 multipart/form-data。");
        }

        var form = await httpContext.Request.ReadFormAsync(cancellationToken);
        var file = form.Files.GetFile(FileField) ?? form.Files.FirstOrDefault()
            ?? throw new UserFriendlyException("上传请求没有携带文件。");
        var input = new FileUploadDto
        {
            File = file,
            RouteKey = Text(form, nameof(FileUploadDto.RouteKey)),
            ProviderName = Text(form, nameof(FileUploadDto.ProviderName)),
            Directory = Text(form, nameof(FileUploadDto.Directory)),
            BucketName = Text(form, nameof(FileUploadDto.BucketName)),
            Overwrite = Flag(form, nameof(FileUploadDto.Overwrite)),
            AccessControl = Text(form, nameof(FileUploadDto.AccessControl)),
            CacheControl = Text(form, nameof(FileUploadDto.CacheControl)),
            AccessPermissions = Text(form, nameof(FileUploadDto.AccessPermissions)),
            IsEncrypted = Flag(form, nameof(FileUploadDto.IsEncrypted)),
            EncryptionKeyId = Text(form, nameof(FileUploadDto.EncryptionKeyId)),
            IsTemporary = Flag(form, nameof(FileUploadDto.IsTemporary)),
            Tags = Text(form, nameof(FileUploadDto.Tags)),
            Remark = Text(form, nameof(FileUploadDto.Remark)),
            ExtendData = Text(form, nameof(FileUploadDto.ExtendData)),
            Width = Int(form, nameof(FileUploadDto.Width)),
            Height = Int(form, nameof(FileUploadDto.Height)),
            Duration = Int(form, nameof(FileUploadDto.Duration)),
            ThumbnailFileId = Long(form, nameof(FileUploadDto.ThumbnailFileId)),
        };
        if (Int(form, nameof(FileUploadDto.RetentionDays)) is { } retentionDays)
        {
            input.RetentionDays = retentionDays;
        }

        if (Text(form, nameof(FileUploadDto.AccessLevel)) is { } accessLevel)
        {
            input.AccessLevel = Enum.TryParse<ResourceAccessLevel>(accessLevel, ignoreCase: true, out var level)
                ? level
                : throw new UserFriendlyException($"访问级别 '{accessLevel}' 无效。");
        }

        if (Text(form, nameof(FileUploadDto.ExpirationTime)) is { } expiration)
        {
            input.ExpirationTime = DateTimeOffset.TryParse(expiration, null, System.Globalization.DateTimeStyles.RoundtripKind, out var time)
                ? time
                : throw new UserFriendlyException($"过期时间 '{expiration}' 无效。");
        }

        var detail = await service.UploadFileAsync(input, cancellationToken);
        return Results.Json(detail, SaasOperationsJsonContext.Default.FileDetailDto);
    }

    /// <summary>
    /// 下载：<c>fileId</c> 来自查询串，按文件记录给出文件名与媒体类型，正文是存储流
    /// </summary>
    private static async Task<IResult> DownloadAsync(HttpContext httpContext, IFileAppService service, IFileQueryService query)
    {
        var cancellationToken = httpContext.RequestAborted;
        if (!long.TryParse(httpContext.Request.Query["fileId"], out var fileId) || fileId <= 0)
        {
            throw new UserFriendlyException("下载请求缺少有效的 fileId。");
        }

        var detail = await query.GetFileDetailAsync(fileId, cancellationToken);
        var stream = await service.DownloadFileAsync(fileId, cancellationToken);
        return Results.Stream(stream, detail?.MimeType ?? "application/octet-stream", detail?.OriginalName ?? detail?.FileName);
    }

    private static string? Text(IFormCollection form, string key)
        => form.TryGetValue(key, out var values) && values.Count > 0 && !string.IsNullOrWhiteSpace(values[0]) ? values[0] : null;

    private static bool Flag(IFormCollection form, string key)
        => Text(form, key) is { } text && bool.TryParse(text, out var value) && value;

    private static int? Int(IFormCollection form, string key)
        => Text(form, key) is { } text && int.TryParse(text, out var value) ? value : null;

    private static long? Long(IFormCollection form, string key)
        => Text(form, key) is { } text && long.TryParse(text, out var value) ? value : null;
}
