// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.Framework.Domain.Events;
using XiHan.Framework.Domain;

namespace XiHan.BasicApp.Core.Events;

/// <summary>
/// 领域事件基类：事件标识与发生时刻
/// </summary>
public abstract class DomainEventBase : IDomainEvent
{
    /// <summary>
    /// 构造函数
    /// </summary>
    protected DomainEventBase()
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// 事件标识
    /// </summary>
    public Guid EventId { get; }

    /// <summary>
    /// 发生时刻
    /// </summary>
    public DateTimeOffset OccurredOn { get; }

    /// <inheritdoc />
    public DateTimeOffset OccurredAt => OccurredOn;

    /// <inheritdoc />
    public override string ToString() => $"{GetType().Name} [EventId: {EventId}, OccurredOn: {OccurredOn:yyyy-MM-dd HH:mm:ss}]";
}
