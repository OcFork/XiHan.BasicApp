// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace XiHan.BasicApp.Core.Data;

/// <summary>
/// 从执行流环境读当前显式工作单元（<see cref="SqlSugarUnitOfWorkManager"/> 用 AsyncLocal 维护），可注册为单例
/// </summary>
public sealed class CurrentUnitOfWorkAccessor : ICurrentUnitOfWorkAccessor
{
    /// <inheritdoc />
    public IUnitOfWork? Current => SqlSugarUnitOfWorkManager.Ambient;
}
