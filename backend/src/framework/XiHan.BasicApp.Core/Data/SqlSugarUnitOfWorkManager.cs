// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using SqlSugar;

namespace XiHan.BasicApp.Core.Data;

/// <summary>
/// 架在作用域 SqlSugar 客户端上的工作单元管理器：最外层按需开事务、完成时提交并执行回调，内层只是加入
/// </summary>
/// <param name="client">作用域内的 SqlSugar 客户端</param>
public sealed class SqlSugarUnitOfWorkManager(ISqlSugarClient client) : IUnitOfWorkManager
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
