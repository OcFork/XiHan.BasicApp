// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.Framework.Kernel.Pipeline;
using XiHan.Framework.Kernel.Security;

namespace XiHan.BasicApp.Core.Security;

/// <summary>
/// 当前租户：先看执行流环境（<see cref="TenantAmbient"/>：显式切换 → 管道登记的调用租户），再退回当前调用的安全上下文
/// </summary>
/// <param name="accessor">当前调用访问器</param>
public sealed class CurrentTenant(IInvocationAccessor accessor) : ICurrentTenant
{
    /// <summary>
    /// 调用携带的租户（没经过管道中间件登记时的退路）
    /// </summary>
    private TenantContext? Invoked => accessor.Current.TryGetValue(out var context) ? context.GetSecurity().Tenant : null;

    /// <inheritdoc />
    public bool IsAvailable => Id is not null;

    /// <inheritdoc />
    public string? Id => TenantAmbient.HasOverride ? TenantAmbient.Id : TenantAmbient.Id ?? Invoked?.Id;

    /// <inheritdoc />
    public string? Name => TenantAmbient.HasOverride ? TenantAmbient.Name : TenantAmbient.Name ?? Invoked?.DisplayName;

    /// <inheritdoc />
    public IDisposable Change(string? id, string? name = null) => TenantAmbient.Override(id, name);
}
