// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.Framework.Domain.Events;
using XiHan.Framework.Domain;

namespace XiHan.BasicApp.Core.Data;

/// <summary>
/// 显式工作单元的管理器
/// </summary>
public interface IUnitOfWorkManager
{
    /// <summary>
    /// 当前工作单元；没有显式开启时为空
    /// </summary>
    IUnitOfWork? Current { get; }

    /// <summary>
    /// 开启工作单元：已有当前工作单元且不要求新的时加入它
    /// </summary>
    /// <param name="options">选项</param>
    /// <param name="requiresNew">是否要求新的边界（SqlSugar 不支持嵌套事务，与外层共用同一连接时仍加入外层）</param>
    IUnitOfWork Begin(XiHanUnitOfWorkOptions? options = null, bool requiresNew = false);

    /// <summary>
    /// 把聚合根缓冲的领域事件发成消息（主题按 <c>DomainEventTopics.For</c>）并清空缓冲；有显式工作单元时推迟到它完成之后，否则立即发布
    /// </summary>
    /// <param name="aggregate">聚合根</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task PublishDomainEventsAsync(IHasDomainEvents aggregate, CancellationToken cancellationToken = default);
}
