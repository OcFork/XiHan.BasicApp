// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text.Json;
using SqlSugar;
using XiHan.BasicApp.Core.Events;
using XiHan.Framework.Domain.Events;
using XiHan.Framework.Domain;
using XiHan.Framework.Messaging.Abstractions.Envelopes;
using XiHan.Framework.Messaging.Abstractions.Publishing;
using XiHan.Framework.Messaging.Abstractions;

namespace XiHan.BasicApp.Core.Data;

/// <summary>
/// 架在作用域 SqlSugar 客户端上的工作单元管理器：最外层按需开事务、完成时提交并执行回调，内层只是加入
/// </summary>
/// <param name="client">作用域内的 SqlSugar 客户端</param>
/// <param name="publisher">消息发布器：领域事件经它发出</param>
public sealed class SqlSugarUnitOfWorkManager(ISqlSugarClient client, IMessagePublisher publisher) : IUnitOfWorkManager
{
    /// <summary>
    /// 当前调用链上的工作单元
    /// </summary>
    private static readonly AsyncLocal<SqlSugarUnitOfWork?> _current = new();

    /// <summary>
    /// 作用域内的客户端（嵌套类型经此访问）
    /// </summary>
    private readonly ISqlSugarClient _client = client;

    /// <inheritdoc />
    public IUnitOfWork? Current => _current.Value;

    /// <summary>
    /// 执行流环境上的当前工作单元（供单例 <see cref="CurrentUnitOfWorkAccessor"/> 读取）
    /// </summary>
    internal static IUnitOfWork? Ambient => _current.Value;

    /// <inheritdoc />
    public IUnitOfWork Begin(XiHanUnitOfWorkOptions? options = null, bool requiresNew = false)
    {
        options ??= new XiHanUnitOfWorkOptions();
        var outer = _current.Value;
        // 外层已经有事务（本层或 5.0 管道的 Transaction 阶段开的）时只能加入：SqlSugar 同一连接不支持嵌套事务
        var ownsTransaction = outer is null && options.IsTransactional && _client.Ado.IsNoTran();
        if (ownsTransaction)
        {
            _client.Ado.BeginTran();
        }

        var unitOfWork = new SqlSugarUnitOfWork(this, outer, options, ownsTransaction);
        _current.Value = unitOfWork;
        return unitOfWork;
    }

    /// <inheritdoc />
    public async Task PublishDomainEventsAsync(IHasDomainEvents aggregate, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(aggregate);
        var events = aggregate.DomainEvents.ToArray();
        aggregate.ClearDomainEvents();
        if (events.Length == 0)
        {
            return;
        }

        // 有显式工作单元就等它完成（提交）后再发；没有时当前就在 5.0 管道的工作单元里，直接发（发件箱特性启用时随事务提交）
        if (_current.Value is { IsCompleted: false } unitOfWork)
        {
            unitOfWork.OnCompleted(() => PublishAsync(events, CancellationToken.None));
            return;
        }

        await PublishAsync(events, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 每个事件一条消息：主题按事件类型推出，数据为事件的 JSON
    /// </summary>
    private async Task PublishAsync(IReadOnlyList<IDomainEvent> events, CancellationToken cancellationToken)
    {
        foreach (var group in events.GroupBy(static domainEvent => DomainEventTopics.For(domainEvent.GetType())))
        {
            var envelopes = group.Select(domainEvent => new MessageEnvelope
            {
                Id = Guid.NewGuid().ToString("N"),
                Source = "/basicapp",
                Type = group.Key,
                Time = domainEvent.OccurredAt,
                Data = JsonSerializer.SerializeToUtf8Bytes(domainEvent, domainEvent.GetType()),
            }).ToList();
            var results = await publisher.PublishManyAsync(group.Key, envelopes, cancellationToken).ConfigureAwait(false);
            var failure = results.FirstOrDefault(static result => result.IsFailure);
            if (failure is { IsFailure: true })
            {
                throw new InvalidOperationException($"Publishing domain event '{group.Key}' failed: {failure.Error}.");
            }
        }
    }

    /// <summary>
    /// 一个工作单元
    /// </summary>
    private sealed class SqlSugarUnitOfWork(SqlSugarUnitOfWorkManager manager, SqlSugarUnitOfWork? outer, XiHanUnitOfWorkOptions options, bool ownsTransaction) : IUnitOfWork
    {
        /// <summary>
        /// 完成回调
        /// </summary>
        private readonly List<Func<Task>> _completed = [];

        /// <summary>
        /// 是否已释放
        /// </summary>
        private bool _disposed;

        /// <inheritdoc />
        public Guid Id { get; } = Guid.NewGuid();

        /// <inheritdoc />
        public XiHanUnitOfWorkOptions Options => options;

        /// <inheritdoc />
        public IUnitOfWork? Outer => outer;

        /// <inheritdoc />
        public bool IsCompleted { get; private set; }

        /// <inheritdoc />
        public async Task CompleteAsync(CancellationToken cancellationToken = default)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            if (IsCompleted)
            {
                return;
            }

            IsCompleted = true;
            if (outer is not null)
            {
                // 内层：回调交给最外层，提交由它负责
                foreach (var handler in _completed)
                {
                    outer.OnCompleted(handler);
                }

                return;
            }

            if (ownsTransaction)
            {
                await manager._client.Ado.CommitTranAsync().ConfigureAwait(false);
            }

            foreach (var handler in _completed)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await handler().ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            IsCompleted = true;
            if (ownsTransaction)
            {
                await manager._client.Ado.RollbackTranAsync().ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public void OnCompleted(Func<Task> handler)
        {
            ArgumentNullException.ThrowIfNull(handler);
            _completed.Add(handler);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            // 没完成就释放：最外层回滚自己开的事务
            if (!IsCompleted && ownsTransaction)
            {
                manager._client.Ado.RollbackTran();
            }

            if (ReferenceEquals(_current.Value, this))
            {
                _current.Value = outer;
            }
        }
    }
}
