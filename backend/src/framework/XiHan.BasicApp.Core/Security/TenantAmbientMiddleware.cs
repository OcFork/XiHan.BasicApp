// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.Framework.Kernel.Pipeline;
using XiHan.Framework.Kernel.Security;
using XiHan.Framework.Kernel;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Core.Security;

/// <summary>
/// 把调用携带的租户登记进执行流环境（<see cref="TenantAmbient"/>），离开调用时恢复；挂在租户解析之后
/// </summary>
public sealed class TenantAmbientMiddleware : IPipelineMiddleware
{
    /// <inheritdoc />
    public async ValueTask InvokeAsync(InvocationContext context, PipelineStep nextStep)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(nextStep);
        var tenant = context.GetSecurity().Tenant;
        // 没有租户（平台调用）也登记一层空值，覆盖外层残留的环境，避免后台线程复用时串租户
        using var scope = TenantAmbient.EnterInvocation(tenant?.Id, tenant?.DisplayName);
        await nextStep(context).ConfigureAwait(false);
    }
}
