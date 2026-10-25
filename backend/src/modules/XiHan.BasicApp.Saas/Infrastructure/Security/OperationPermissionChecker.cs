// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using XiHan.BasicApp.Core.Authorization.Permissions;
using XiHan.Framework.Kernel.Security;
using IPermissionChecker = XiHan.BasicApp.Core.Authorization.Permissions.IPermissionChecker;

namespace XiHan.BasicApp.Saas.Infrastructure.Security;

/// <summary>
/// 内核权限策略的判定实现：把操作契约上的权限码交给应用自己的 <see cref="IPermissionChecker"/>
/// （授权快照、会话有效性、模仿态禁用清单与权限条件都在那里判定，并按请求作用域记忆化）
/// </summary>
/// <remarks>
/// 内核的 <c>PolicyCatalog</c> 是单例、只能抱着单例检查器，而应用检查器随请求作用域：
/// 请求内从当前 HttpContext 的 RequestServices 取，没有 HTTP 请求（进程内调用、后台任务）时临时开作用域，租户仍由执行流环境提供。
/// 匿名主体由内核 <c>PermissionPolicy</c> 先行拒绝，这里不再判。
/// </remarks>
public sealed class OperationPermissionChecker : XiHan.Framework.Security.Abstractions.IPermissionChecker
{
    private readonly IServiceProvider _rootServices;

    /// <summary>
    /// 创建检查器
    /// </summary>
    /// <param name="rootServices">根容器</param>
    public OperationPermissionChecker(IServiceProvider rootServices)
    {
        ArgumentNullException.ThrowIfNull(rootServices);
        _rootServices = rootServices;
    }

    /// <inheritdoc />
    public async ValueTask<bool> HasPermissionAsync(SecurityContext security, string permission, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(security);
        ArgumentException.ThrowIfNullOrWhiteSpace(permission);
        if (!security.Principal.IsAuthenticated)
        {
            return false;
        }

        var httpContext = _rootServices.GetService<IHttpContextAccessor>()?.HttpContext;
        if (httpContext is not null)
        {
            return await CheckAsync(httpContext.RequestServices, security.Principal.Id, permission, httpContext, cancellationToken).ConfigureAwait(false);
        }

        await using var scope = _rootServices.CreateAsyncScope();
        return await CheckAsync(scope.ServiceProvider, security.Principal.Id, permission, null, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 用给定作用域的应用检查器判定
    /// </summary>
    private static async ValueTask<bool> CheckAsync(IServiceProvider services, string userId, string permission, HttpContext? httpContext, CancellationToken cancellationToken)
    {
        var checker = services.GetRequiredService<IPermissionChecker>();
        var result = await checker.CheckAsync(new PermissionCheckContext(userId, permission, resource: httpContext), cancellationToken).ConfigureAwait(false);
        return result.IsGranted;
    }
}
