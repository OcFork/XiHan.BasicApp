// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Core.Entities;
using XiHan.Framework.Domain;

namespace XiHan.BasicApp.Core.Aggregates;

/// <summary>
/// 聚合根接口：全审计实体 + 领域事件缓冲（5.0 的 <see cref="IHasDomainEvents"/>）
/// </summary>
public interface IAggregateRoot : IFullAuditedEntity, IHasDomainEvents
{
}

/// <summary>
/// 泛型主键聚合根接口；同时是 5.0 的 <see cref="XiHan.Framework.Domain.IAggregateRoot{TKey}"/>，仓储按它发布领域事件
/// </summary>
/// <typeparam name="TKey">主键类型</typeparam>
public interface IAggregateRoot<TKey> : IAggregateRoot, IFullAuditedEntity<TKey>, XiHan.Framework.Domain.IAggregateRoot<TKey>
    where TKey : struct, IEquatable<TKey>
{
}
