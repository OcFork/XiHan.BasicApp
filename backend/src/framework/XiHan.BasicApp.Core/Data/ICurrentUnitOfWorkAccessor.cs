// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace XiHan.BasicApp.Core.Data;

/// <summary>
/// 当前执行流上的显式工作单元（只读）；单例服务用它把副作用推迟到工作单元提交之后，而不必依赖作用域的 <see cref="IUnitOfWorkManager"/>
/// </summary>
public interface ICurrentUnitOfWorkAccessor
{
    /// <summary>
    /// 当前工作单元；没有显式开启时为 null
    /// </summary>
    IUnitOfWork? Current { get; }
}
