// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Core.Data;
using XiHan.BasicApp.Core.Aggregates;
using XiHan.BasicApp.Core.Repositories;

namespace XiHan.BasicApp.Core.Data.SqlSugar.Repository;

/// <summary>
/// SqlSugar 聚合根仓储实现
/// </summary>
/// <typeparam name="TAggregateRoot">聚合根类型</typeparam>
/// <typeparam name="TKey">主键类型</typeparam>
public class SqlSugarAggregateRepository<TAggregateRoot, TKey> : SqlSugarAuditedRepository<TAggregateRoot, TKey>, IAggregateRootRepository<TAggregateRoot, TKey>
    where TAggregateRoot : class, IAggregateRoot<TKey>, new()
    where TKey : struct, IEquatable<TKey>
{
    // xihan-migrate(V4-05): IUnitOfWorkManager 没有 5.0 对应物：每个操作默认一个工作单元，需要显式控制时注入作用域的 IUnitOfWork。
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="clientResolver">SqlSugar 客户端解析器</param>
    /// <param name="unitOfWorkManager">工作单元管理器</param>
    public SqlSugarAggregateRepository(
        ISqlSugarClientResolver clientResolver,
        IUnitOfWorkManager unitOfWorkManager)
        : base(clientResolver)
    {
        _unitOfWorkManager = unitOfWorkManager;
    }

    /// <summary>
    /// 工作单元（无活动 UoW 时返回 null）
    /// </summary>
    protected IUnitOfWork? UnitOfWork => _unitOfWorkManager.Current;

    /// <summary>
    /// 添加聚合根并触发聚合根上的领域事件
    /// </summary>
    /// <param name="aggregate">聚合根实例</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>已持久化的聚合根实例</returns>
    public override async Task<TAggregateRoot> AddAsync(TAggregateRoot aggregate, CancellationToken cancellationToken = default)
    {
        var result = await base.AddAsync(aggregate, cancellationToken);
        await PublishDomainEventsAsync(aggregate, cancellationToken);
        return result;
    }

    /// <summary>
    /// 更新聚合根并触发聚合根上的领域事件
    /// </summary>
    /// <param name="aggregate">聚合根实例</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>更新后的聚合根实例</returns>
    public override async Task<TAggregateRoot> UpdateAsync(TAggregateRoot aggregate, CancellationToken cancellationToken = default)
    {
        var result = await base.UpdateAsync(aggregate, cancellationToken);
        await PublishDomainEventsAsync(aggregate, cancellationToken);
        return result;
    }

    /// <summary>
    /// 删除聚合根并触发聚合根上的领域事件
    /// </summary>
    /// <param name="aggregate">聚合根实例</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>删除是否成功</returns>
    public override async Task<bool> DeleteAsync(TAggregateRoot aggregate, CancellationToken cancellationToken = default)
    {
        var deleted = await base.DeleteAsync(aggregate, cancellationToken);
        await PublishDomainEventsAsync(aggregate, cancellationToken);
        return deleted;
    }

    /// <summary>
    /// 保存聚合根变更（包括事件处理）
    /// </summary>
    /// <param name="aggregate">需要持久化的聚合根实例</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>表示保存操作的任务</returns>
    public async Task SaveAggregateAsync(TAggregateRoot aggregate, CancellationToken cancellationToken = default)
    {
        if (aggregate.IsTransient())
        {
            await AddAsync(aggregate, cancellationToken);
        }
        else
        {
            await UpdateAsync(aggregate, cancellationToken);
        }
    }

    /// <summary>
    /// 获取聚合根及其相关实体
    /// </summary>
    /// <param name="id">聚合根主键</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>聚合根实例，如果不存在则返回 <c>null</c></returns>
    public Task<TAggregateRoot?> GetAggregateAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return GetByIdAsync(id, cancellationToken);
    }

    /// <summary>
    /// 获取包含领域事件的聚合根
    /// </summary>
    /// <param name="id">聚合根主键</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>包含未发布领域事件的聚合根实例，如果不存在则返回 <c>null</c></returns>
    public async Task<TAggregateRoot?> GetWithEventsAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var aggregate = await GetByIdAsync(id, cancellationToken);
        return aggregate;
    }

    /// <summary>
    /// 聚合根缓冲的领域事件经工作单元管理器发成消息（显式工作单元完成后、否则立即），并清空缓冲
    /// </summary>
    /// <param name="aggregate">聚合根</param>
    /// <param name="cancellationToken">取消令牌</param>
    private Task PublishDomainEventsAsync(TAggregateRoot aggregate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(aggregate);
        return aggregate.DomainEvents.Count == 0 ? Task.CompletedTask : _unitOfWorkManager.PublishDomainEventsAsync(aggregate, cancellationToken);
    }
}
