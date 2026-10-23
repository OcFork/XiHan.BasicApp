// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace XiHan.BasicApp.Core.Data;

/// <summary>
/// 应用层显式开启的工作单元
/// </summary>
/// <remarks>
/// 5.0 的每个操作在 Transaction 阶段默认就有一个工作单元；这里只给仍需显式控制边界的少数代码用，
/// 内层工作单元加入外层（完成时不提交），只有最外层在完成时提交事务并触发完成回调。
/// </remarks>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// 标识
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// 选项
    /// </summary>
    XiHanUnitOfWorkOptions Options { get; }

    /// <summary>
    /// 外层工作单元；最外层为空
    /// </summary>
    IUnitOfWork? Outer { get; }

    /// <summary>
    /// 是否已完成
    /// </summary>
    bool IsCompleted { get; }

    /// <summary>
    /// 完成：最外层提交事务并依次执行完成回调
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    Task CompleteAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 回滚：最外层回滚事务
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    Task RollbackAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 登记完成回调（在最外层提交之后执行）
    /// </summary>
    /// <param name="handler">回调</param>
    void OnCompleted(Func<Task> handler);
}
