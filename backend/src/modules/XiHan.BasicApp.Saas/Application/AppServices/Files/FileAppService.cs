// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using XiHan.BasicApp.Saas.Application.Contracts;
using XiHan.BasicApp.Saas.Application.Dtos;
using XiHan.BasicApp.Saas.Application.Mappers;
using XiHan.BasicApp.Saas.Application.QueryServices;
using XiHan.BasicApp.Saas.Application.Services;
using XiHan.BasicApp.Saas.Domain.DomainServices;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Events;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Core.Security;
using XiHan.Framework.Messaging.Abstractions;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Application.AppServices;

/// <summary>
/// 系统文件命令应用服务
/// </summary>
public sealed class FileAppService
    : SaasApplicationService, IFileAppService
{
    private readonly ICurrentUser _currentUser;

    private readonly IFileDomainService _fileDomainService;

    private readonly IFileRecordQueryService _fileRecordQueryService;

    private readonly IFileTransferService _fileTransferService;

    private readonly IMessagePublisher _localEventBus;

    private readonly IFieldSecurityService _fieldSecurity;

    /// <summary>
    /// 构造函数
    /// </summary>
    public FileAppService(
        IFileDomainService fileDomainService,
        IFileRecordQueryService fileRecordQueryService,
        IFileTransferService fileTransferService,
        IMessagePublisher localEventBus,
        ICurrentUser currentUser,
        IFieldSecurityService fieldSecurity)
    {
        _fileDomainService = fileDomainService;
        _fileRecordQueryService = fileRecordQueryService;
        _fileTransferService = fileTransferService;
        _localEventBus = localEventBus;
        _currentUser = currentUser;
        _fieldSecurity = fieldSecurity;
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    public async Task DeleteFileAsync(FileDeleteDto input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);
        cancellationToken.ThrowIfCancellationRequested();

        var file = await _fileRecordQueryService.GetFileOrThrowAsync(input.Id, cancellationToken);
        var storages = await _fileRecordQueryService.GetStoragesByFileIdAsync(file.Id, cancellationToken);

        if (input.DeletePhysical)
        {
            await _fileTransferService.DeletePhysicalAsync(storages, cancellationToken);
        }

        var result = await _fileDomainService.DeleteFileAsync(FileApplicationMapper.ToDeleteCommand(input), cancellationToken);

        // xihan-migrate(V4-08): 事件用基于反射的 System.Text.Json 序列化；把事件类型登记进 JsonSerializerContext 并传它的 JsonTypeInfo 才能 AOT。
        await _localEventBus.PublishAsync("file-deleted", new MessageEnvelope { Id = Guid.NewGuid().ToString("N"), Source = "/saas", Type = "file-deleted", Time = DateTimeOffset.UtcNow, Data = JsonSerializer.SerializeToUtf8Bytes(new FileDeletedDomainEvent(
                result.File.TenantId,
                result.File.Id,
                result.File.FileName,
                input.DeletePhysical,
                _currentUser.UserId,
                input.Reason)) }, cancellationToken);
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <remarks>
    /// 事务不是为了"读"，而是为了那次下载计数：下载失败（主存储不可达/流打开失败）时
    /// 计数应当一并回滚，否则统计会把打不开的文件也算成一次下载。
    /// 注意裸 <c>[UnitOfWork]</c> 实际不开事务（IsTransactional 可空，不传即不覆盖，选项默认非事务），
    /// 所以这里必须显式写 true。
    /// </remarks>
    // xihan-migrate(V4-06): DownloadFileAsync 没法变成操作（泛型、ref/out 参数、流或 ASP.NET 类型）；请手工暴露。
    public async Task<Stream> DownloadFileAsync(long fileId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var storage = await _fileRecordQueryService.GetPrimaryStorageOrThrowAsync(fileId, "文件缺少可用主存储，无法下载。", cancellationToken);

        // 计入下载次数（用户发起下载即记一次）
        await _fileDomainService.IncrementDownloadCountAsync(fileId, cancellationToken);

        return await _fileTransferService.DownloadAsync(storage, cancellationToken);
    }

    /// <summary>
    /// 秒传文件（按哈希探测：未命中返回 null，前端回退普通上传）
    /// </summary>
    public async Task<FileDetailDto?> FastUploadFileAsync(FileFastUploadDto input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);
        cancellationToken.ThrowIfCancellationRequested();

        var result = await _fileDomainService.FastUploadFileAsync(FileApplicationMapper.ToFastUploadCommand(input), cancellationToken);
        return result is null ? null : FileApplicationMapper.ToDetailDto(result.File);
    }

    /// <summary>
    /// 生成文件预签名访问 URL
    /// </summary>
    /// <remarks>
    /// 与下载同理：签名链接生成失败时，那次访问计数应当一并回滚。
    /// 裸 <c>[UnitOfWork]</c> 读起来像开事务、实际不开，故显式声明为事务性。
    /// </remarks>
    public async Task<string> GenerateFilePresignedUrlAsync(long fileId, TimeSpan? expiresIn = null, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var storage = await _fileRecordQueryService.GetPrimaryStorageOrThrowAsync(fileId, "文件缺少可用主存储，无法生成访问链接。", cancellationToken);

        // 计入访问次数（生成预签名访问链接即记一次）
        await _fileDomainService.IncrementViewCountAsync(fileId, cancellationToken);

        return await _fileTransferService.GeneratePresignedUrlAsync(storage, expiresIn, cancellationToken);
    }

    /// <summary>
    /// 切换主存储
    /// </summary>
    public async Task<FileStorageDetailDto> SwitchPrimaryStorageAsync(FilePrimaryStorageSwitchDto input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);
        cancellationToken.ThrowIfCancellationRequested();

        var result = await _fileDomainService.SwitchPrimaryStorageAsync(
            FileApplicationMapper.ToPrimaryStorageSwitchCommand(input),
            cancellationToken);

        // xihan-migrate(V4-08): 事件用基于反射的 System.Text.Json 序列化；把事件类型登记进 JsonSerializerContext 并传它的 JsonTypeInfo 才能 AOT。
        await _localEventBus.PublishAsync("file-primary-storage-changed", new MessageEnvelope { Id = Guid.NewGuid().ToString("N"), Source = "/saas", Type = "file-primary-storage-changed", Time = DateTimeOffset.UtcNow, Data = JsonSerializer.SerializeToUtf8Bytes(new FilePrimaryStorageChangedDomainEvent(
                result.Storage.TenantId,
                result.Storage.FileId,
                result.Storage.Id,
                result.PreviousStorageId,
                _currentUser.UserId,
                input.Remark)) }, cancellationToken);

        return FileApplicationMapper.ToStorageDetailDto(result.Storage);
    }

    /// <summary>
    /// 更新文件业务元数据
    /// </summary>
    public async Task<FileDetailDto> UpdateFileMetadataAsync(FileMetadataUpdateDto input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);
        cancellationToken.ThrowIfCancellationRequested();

        // 字段安全：只读字段不能改，表单交回的脱敏值还原为原值
        await _fieldSecurity.EnsureUpdatableAsync(typeof(SysFile), input.Id, input, cancellationToken);

        var result = await _fileDomainService.UpdateFileMetadataAsync(FileApplicationMapper.ToMetadataUpdateCommand(input), cancellationToken);
        return FileApplicationMapper.ToDetailDto(result.File);
    }

    /// <summary>
    /// 更新系统文件状态
    /// </summary>
    public async Task<FileDetailDto> UpdateFileStatusAsync(FileStatusUpdateDto input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);
        cancellationToken.ThrowIfCancellationRequested();

        // 字段安全：只读字段不能改，表单交回的脱敏值还原为原值
        await _fieldSecurity.EnsureUpdatableAsync(typeof(SysFile), input.Id, input, cancellationToken);

        var result = await _fileDomainService.UpdateFileStatusAsync(
            FileApplicationMapper.ToStatusCommand(input),
            cancellationToken);
        return FileApplicationMapper.ToDetailDto(result.File);
    }

    /// <summary>
    /// 更新系统文件存储状态
    /// </summary>
    public async Task<FileStorageDetailDto> UpdateFileStorageStatusAsync(FileStorageStatusUpdateDto input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);
        cancellationToken.ThrowIfCancellationRequested();

        // 字段安全：只读字段不能改，表单交回的脱敏值还原为原值
        await _fieldSecurity.EnsureUpdatableAsync(typeof(SysFileStorage), input.Id, input, cancellationToken);

        var result = await _fileDomainService.UpdateFileStorageStatusAsync(
            FileApplicationMapper.ToStorageStatusCommand(input),
            cancellationToken);
        return FileApplicationMapper.ToStorageDetailDto(result.Storage);
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    public async Task<FileDetailDto> UploadFileAsync(FileUploadDto input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);
        cancellationToken.ThrowIfCancellationRequested();

        var result = await _fileTransferService.UploadAsync(input, cancellationToken);
        if (result.Uploaded && result.Storage is not null)
        {
            await PublishUploadedAsync(result.File, result.Storage, result.Reason, cancellationToken);
        }

        return FileApplicationMapper.ToDetailDto(result.File);
    }

    /// <summary>
    /// 校验文件存储副本
    /// </summary>
    public async Task<FileStorageDetailDto> VerifyFileStorageAsync(FileStorageVerifyDto input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);
        cancellationToken.ThrowIfCancellationRequested();

        var storage = await _fileRecordQueryService.GetStorageOrThrowAsync(input.Id, cancellationToken);
        var probe = await _fileTransferService.ProbeStorageAsync(storage, cancellationToken);

        var result = await _fileDomainService.VerifyFileStorageAsync(
            FileApplicationMapper.ToStorageVerifyCommand(input, probe),
            cancellationToken);
        return FileApplicationMapper.ToStorageDetailDto(result.Storage);
    }

    private async Task PublishUploadedAsync(SysFile file, SysFileStorage storage, string? reason, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // xihan-migrate(V4-08): 事件用基于反射的 System.Text.Json 序列化；把事件类型登记进 JsonSerializerContext 并传它的 JsonTypeInfo 才能 AOT。
        await _localEventBus.PublishAsync("file-uploaded", new MessageEnvelope { Id = Guid.NewGuid().ToString("N"), Source = "/saas", Type = "file-uploaded", Time = DateTimeOffset.UtcNow, Data = JsonSerializer.SerializeToUtf8Bytes(new FileUploadedDomainEvent(
                file.TenantId,
                file.Id,
                storage.Id,
                file.FileName,
                file.FileSize,
                _currentUser.UserId,
                reason)) }, cancellationToken);
    }
}
