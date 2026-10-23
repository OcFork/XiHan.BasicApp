// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Data;

namespace XiHan.BasicApp.Core.Data;

/// <summary>
/// 显式工作单元的选项
/// </summary>
/// <param name="isTransactional">是否开启数据库事务</param>
/// <param name="isolationLevel">隔离级别</param>
/// <param name="timeout">超时（毫秒）</param>
public sealed class XiHanUnitOfWorkOptions(bool isTransactional = false, IsolationLevel? isolationLevel = null, int? timeout = null)
{
    /// <summary>
    /// 是否开启数据库事务
    /// </summary>
    public bool IsTransactional { get; } = isTransactional;

    /// <summary>
    /// 隔离级别
    /// </summary>
    public IsolationLevel? IsolationLevel { get; } = isolationLevel;

    /// <summary>
    /// 超时（毫秒）
    /// </summary>
    public int? Timeout { get; } = timeout;
}
