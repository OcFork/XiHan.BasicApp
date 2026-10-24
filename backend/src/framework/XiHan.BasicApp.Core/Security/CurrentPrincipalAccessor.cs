// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Security.Claims;
using XiHan.Framework.Kernel.Pipeline;
using XiHan.Framework.Kernel.Security;

namespace XiHan.BasicApp.Core.Security;

/// <summary>
/// 当前主体访问器：有执行流覆盖（<see cref="PrincipalAmbient"/>）时读覆盖，否则把调用携带的 5.0 主体转成 ClaimsPrincipal；
/// <see cref="Change"/> 登记覆盖，供后台任务以指定用户身份执行
/// </summary>
/// <param name="accessor">当前调用</param>
public sealed class CurrentPrincipalAccessor(IInvocationAccessor accessor) : ICurrentPrincipalAccessor
{
    /// <inheritdoc />
    public ClaimsPrincipal Principal => PrincipalAmbient.Current
        ?? PrincipalConversion.ToClaimsPrincipal(accessor.Current.TryGetValue(out var context) ? context.GetSecurity().Principal : Framework.Kernel.Security.Principal.Anonymous);

    /// <inheritdoc />
    public IDisposable Change(ClaimsPrincipal principal) => PrincipalAmbient.Override(principal);
}
