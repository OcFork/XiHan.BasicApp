// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text.Json;
using Microsoft.Extensions.Logging;
using XiHan.BasicApp.Core.Data;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Events;
using XiHan.Framework.Messaging.Abstractions.Handlers;
using XiHan.Framework.Messaging.Abstractions;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Application.EventHandlers;

/// <summary>
/// 文件上传完成事件处理器
/// </summary>
/// <remarks>
/// 当文件上传完成后，创建主存储记录并触发病毒扫描（桩实现）。
/// </remarks>
public sealed class FileUploadedEventHandler : IMessageHandler
{
    /// <summary>由 xihan migrate v4 生成；提交前请检查。</summary>
    public const string FileUploadedTopic = "file-uploaded";

    /// <inheritdoc />
    public async ValueTask<Result> HandleAsync(MessageDelivery delivery, InvocationContext context, CancellationToken cancellationToken)
    {
        // xihan-migrate(V4-08): 事件用基于反射的 System.Text.Json 序列化；把事件类型登记进 JsonSerializerContext 并传它的 JsonTypeInfo 才能 AOT。
        await HandleEventAsync(JsonSerializer.Deserialize<FileUploadedDomainEvent>(delivery.Envelope.Data.Span)!).ConfigureAwait(false);
        return Result.Success();
    }

    private readonly ISqlSugarClientResolver _clientResolver;
    private readonly ILogger<FileUploadedEventHandler> _logger;

    /// <summary>
    /// 构造函数
    /// </summary>
    public FileUploadedEventHandler(
        ISqlSugarClientResolver clientResolver,
        ILogger<FileUploadedEventHandler> logger)
    {
        _clientResolver = clientResolver ?? throw new ArgumentNullException(nameof(clientResolver));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// 处理文件上传完成事件
    /// </summary>
    /// <param name="eventData">事件数据</param>
    public async Task HandleEventAsync(FileUploadedDomainEvent eventData)
    {
        ArgumentNullException.ThrowIfNull(eventData);

        _logger.LogInformation(
            "[FileUploaded] File uploaded: FileId={FileId}, StorageId={StorageId}, FileName={FileName}, Size={FileSize}",
            eventData.FileId, eventData.StorageId, eventData.FileName, eventData.FileSize);

        // 确保主存储记录已创建（大部分场景下上传服务已完成此操作，此处为幂等保护）
        await EnsurePrimaryStorageAsync(eventData);

        // 触发病毒扫描桩
        await TriggerVirusScanStubAsync(eventData);
    }

    /// <summary>
    /// 确保主存储记录存在（幂等）
    /// </summary>
    private async Task EnsurePrimaryStorageAsync(FileUploadedDomainEvent eventData)
    {
        try
        {
            var db = _clientResolver.GetCurrentClient();

            var existing = await db.Queryable<SysFileStorage>()
                .Where(s => s.Id == eventData.StorageId && !s.IsDeleted)
                .FirstAsync();

            if (existing is not null)
            {
                // 存储记录已存在，标记为主存储
                if (!existing.IsPrimary)
                {
                    existing.IsPrimary = true;
                    existing.Status = FileStorageStatus.Normal;
                    await db.Updateable(existing)
                        .UpdateColumns(s => new { s.IsPrimary, s.Status })
                        .ExecuteCommandAsync();
                }

                _logger.LogDebug(
                    "[FileUploaded] Primary storage record verified: StorageId={StorageId}", eventData.StorageId);
                return;
            }

            // 存储记录不存在，创建新记录
            var now = DateTimeOffset.UtcNow;
            var storage = new SysFileStorage
            {
                FileId = eventData.FileId,
                StorageType = FileStorageType.Local,
                StoragePath = eventData.FileName,
                IsPrimary = true,
                Status = FileStorageStatus.Normal,
                UploadedTime = now,
                TenantId = eventData.TenantId
            };

            await db.Insertable(storage).ExecuteCommandAsync();

            _logger.LogInformation(
                "[FileUploaded] Created primary storage record: StorageId={StorageId}, FileId={FileId}",
                eventData.StorageId, eventData.FileId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "[FileUploaded] Failed to ensure primary storage for FileId={FileId}, StorageId={StorageId}",
                eventData.FileId, eventData.StorageId);
        }
    }

    /// <summary>
    /// 触发病毒扫描（桩实现）
    /// </summary>
    /// <remarks>
    /// 当前为桩实现，后续可接入 ClamAV 或其他病毒扫描服务。
    /// </remarks>
    private Task TriggerVirusScanStubAsync(FileUploadedDomainEvent eventData)
    {
        _logger.LogInformation(
            "[FileUploaded][VirusScan] Virus scan stub triggered for FileId={FileId}, FileName={FileName}",
            eventData.FileId, eventData.FileName);

        // TODO: 接入真实病毒扫描服务 (ClamAV / 第三方安全服务)
        // 1. 将文件推送至病毒扫描队列
        // 2. 扫描完成后通过回调或事件更新 SysFile.Status 和 SysFileStorage.Status
        // 3. 若发现病毒，标记文件为已损坏/已隔离，并通知上传者

        return Task.CompletedTask;
    }
}
