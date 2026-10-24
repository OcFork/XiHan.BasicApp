// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Http;
using XiHan.BasicApp.Core.Authorization.Permissions;
using XiHan.Framework.Kernel.Security;

namespace XiHan.BasicApp.Saas.Infrastructure.Security;

/// <summary>
/// 5.0 操作授权的权限检查器：把操作契约上的权限码交给应用自己的 <see cref="IPermissionChecker"/>
/// （授权快照、会话有效性、模仿态禁用清单与权限条件都在那里判定，并按请求作用域记忆化）
/// </summary>
/// <param name="checker">应用的权限检查器（作用域）</param>
/// <param name="httpContextAccessor">当前请求；权限条件（IP、时段等）按它求值，没有请求时不求条件</param>
public sealed class OperationPermissionChecker(IPermissionChecker checker, IHttpContextAccessor httpContextAccessor)
    : XiHan.Framework.Security.Abstractions.IPermissionChecker
{
    /// <inheritdoc />
    public async ValueTask<bool> HasPermissionAsync(SecurityContext security, string permission, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(security);
        ArgumentException.ThrowIfNullOrWhiteSpace(permission);
        // 匿名主体没有权限；主体标识就是用户主键，应用检查器自己解析
        if (!security.Principal.IsAuthenticated)
        {
            return false;
        }

        var context = new PermissionCheckContext(security.Principal.Id, permission, resource: httpContextAccessor.HttpContext);
        var result = await checker.CheckAsync(context, cancellationToken).ConfigureAwait(false);
        return result.IsGranted;
    }
}
