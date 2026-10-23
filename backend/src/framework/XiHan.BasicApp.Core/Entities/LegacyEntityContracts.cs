// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.Framework.Domain;

namespace XiHan.BasicApp.Core.Entities;

/// <summary>
/// 带并发戳的实体（4.x <c>IEntityBase</c> 的应用层对应物；<c>RowVersion</c> 已按 5.0 协议改为字符串并发戳）
/// </summary>
public interface IEntityBase : IHasConcurrencyStamp
{
}

/// <summary>
/// 带标识与并发戳的实体
/// </summary>
/// <typeparam name="TKey">标识类型</typeparam>
public interface IEntityBase<out TKey> : IEntityBase, IEntity<TKey>
{
}

/// <summary>
/// 软删除标记（等同 5.0 的 <see cref="ISoftDeletable"/>，保留 4.x 名字）
/// </summary>
public interface ISoftDelete : ISoftDeletable
{
}

/// <summary>
/// 分表实体：按创建时间分表的日志类实体
/// </summary>
public interface ISplitTableEntity : IHasCreationTime
{
}

/// <summary>
/// 可追踪实体：记录写入它的调用链标识
/// </summary>
public interface ITraceableEntity
{
    /// <summary>
    /// 追踪标识
    /// </summary>
    string? TraceId { get; set; }
}

/// <summary>
/// 当前追踪标识提供者
/// </summary>
public interface ITraceIdProvider
{
    /// <summary>
    /// 取当前调用的追踪标识；没有在调用里时为空
    /// </summary>
    string? GetCurrentTraceId();
}
