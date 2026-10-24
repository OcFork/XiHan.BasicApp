// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Core.Tenancy;

namespace XiHan.BasicApp.Core.Security;

/// <summary>
/// 当前租户访问器（单例）：给 SqlSugar 全局过滤器这类按查询即时求值、又不在作用域里的地方读当前租户
/// </summary>
public interface ICurrentTenantAccessor
{
    /// <summary>
    /// 当前租户；没有租户上下文（平台）时为空
    /// </summary>
    BasicTenantInfo? Current { get; }
}
