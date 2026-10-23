// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Logging;
using System.Text.Json;
using XiHan.BasicApp.Saas.Domain.Entities;
using XiHan.BasicApp.Saas.Domain.Events;
using XiHan.Framework.Messaging.Abstractions;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Saas.Application.EventHandlers;

/// <summary>
/// 文件主存储切换事件处理器
/// </summary>
/// <remarks>
/// 当文件的主存储发生切换（如从本地迁移到 OSS）时，记录审计日志。
/// </remarks>
public sealed class FilePrimaryStorageChangedEventHandler : IMessageHandler
{
    /// <summary>由 xihan migrate v4 生成；提交前请检查。</summary>
    public const string FilePrimaryStorageChangedTopic = "file-primary-storage-changed";

    /// <inheritdoc />
    public async ValueTask<Result> HandleAsync(MessageDelivery delivery, InvocationContext context, CancellationToken cancellationToken)
    {
        // xihan-migrate(V4-08): 事件用基于反射的 System.Text.Json 序列化；把事件类型登记进 JsonSerializerContext 并传它的 JsonTypeInfo 才能 AOT。
        await HandleEventAsync(JsonSerializer.Deserialize<FilePrimaryStorageChangedDomainEvent>(delivery.Envelope.Data.Span)!).ConfigureAwait(false);
        return Result.Success();
    }

    private readonly ISqlSugarClientResolver _clientResolver;
    private readonly ILogger<FilePrimaryStorageChangedEventHandler> _logger;

    /// <summary>
    /// 构造函数
    /// </summary>
    public FilePrimaryStorageChangedEventHandler(
        ISqlSugarClientResolver clientResolver,
        ILogger<FilePrimaryStorageChangedEventHandler> logger)
    {
        _clientResolver = clientResolver ?? throw new ArgumentNullException(nameof(clientResolver));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// 处理文件主存储切换事件
    /// </summary>
    /// <param name="eventData">事件数据</param>
    public async Task HandleEventAsync(FilePrimaryStorageChangedDomainEvent eventData)
    {
        ArgumentNullException.ThrowIfNull(eventData);

        _logger.LogInformation(
            "[FilePrimaryStorageChanged] Primary storage changed for FileId={FileId}: NewStorageId={StorageId}, PreviousStorageId={PreviousStorageId}",
            eventData.FileId, eventData.StorageId, eventData.PreviousStorageId);

        try
        {
            var db = _clientResolver.GetCurrentClient();

            // 将原主存储标记为非主存储
            if (eventData.PreviousStorageId.HasValue)
            {
                await db.Updateable<SysFileStorage>()
                    .SetColumns(s => s.IsPrimary == false)
                    .Where(s => s.Id == eventData.PreviousStorageId.Value)
                    .ExecuteCommandAsync();

                _logger.LogDebug(
                    "[FilePrimaryStorageChanged] Previous storage {PreviousStorageId} demoted from primary",
                    eventData.PreviousStorageId);
            }

            // 确保新主存储标记为主存储
            await db.Updateable<SysFileStorage>()
                .SetColumns(s => s.IsPrimary == true)
                .Where(s => s.Id == eventData.StorageId)
                .ExecuteCommandAsync();

            _logger.LogInformation(
                "[FilePrimaryStorageChanged] Storage {StorageId} promoted to primary for FileId={FileId}",
                eventData.StorageId, eventData.FileId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "[FilePrimaryStorageChanged] Failed to update storage primary status for FileId={FileId}",
                eventData.FileId);
        }
    }
}
