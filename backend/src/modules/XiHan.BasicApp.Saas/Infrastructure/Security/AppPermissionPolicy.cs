// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using XiHan.BasicApp.Core.Authorization.Permissions;
using XiHan.Framework.Kernel.Security;
using XiHan.Framework.Primitives;
using XiHan.Framework.Security.Abstractions;
using IPermissionChecker = XiHan.BasicApp.Core.Authorization.Permissions.IPermissionChecker;

namespace XiHan.BasicApp.Saas.Infrastructure.Security;

/// <summary>
/// 应用权限码对应的 5.0 授权策略（策略名 <c>permission:&lt;code&gt;</c>）：把判定交给应用自己的 <see cref="IPermissionChecker"/>
/// （授权快照、会话有效性、模仿态禁用清单与权限条件都在那里，并按请求作用域记忆化）
/// </summary>
/// <remarks>
/// 应用的权限码是 4.x 的冒号形式（<c>saas:tenant:read</c>），不符合内核 <c>PermissionRegistry</c> 要求的点分文法，
/// 所以不走「登记权限定义 → 动态权限策略」那条路，而是按名注册同名策略：策略目录先查注册的策略，再查权限注册表。
/// </remarks>
public sealed class AppPermissionPolicy : IPolicy
{
    private readonly IServiceProvider _rootServices;

    /// <summary>
    /// 创建策略
    /// </summary>
    /// <param name="permissionCode">应用权限码</param>
    /// <param name="rootServices">根容器；判定时优先用当前请求的作用域，没有请求再临时开作用域</param>
    public AppPermissionPolicy(string permissionCode, IServiceProvider rootServices)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(permissionCode);
        ArgumentNullException.ThrowIfNull(rootServices);
        PermissionCode = permissionCode;
        _rootServices = rootServices;
    }

    /// <summary>
    /// 应用权限码
    /// </summary>
    public string PermissionCode { get; }

    /// <inheritdoc />
    public string Name => PermissionPolicy.NameFor(PermissionCode);

    /// <inheritdoc />
    public async ValueTask<AuthorizationResult> EvaluateAsync(AuthorizationRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        // 先判未认证：匿名主体得到 NotAuthenticated 而不是 PermissionMissing，调用方可以区分未登录与越权
        if (!request.Security.Principal.IsAuthenticated)
        {
            return AuthorizationResult.Deny(Error.Denied(SecurityErrors.NotAuthenticated, SecurityMessages.AuthenticationRequired));
        }

        // 请求内判定用请求自己的作用域：检查器按请求记忆化快照与会话校验，当前用户、租户也都在那个作用域里；
        // 没有 HTTP 请求（进程内调用、后台任务）时临时开一个作用域，租户仍由执行流环境提供
        var httpContext = _rootServices.GetService<IHttpContextAccessor>()?.HttpContext;
        var granted = httpContext is not null
            ? await CheckAsync(httpContext.RequestServices, request.Security.Principal.Id, httpContext, cancellationToken).ConfigureAwait(false)
            : await CheckInNewScopeAsync(request.Security.Principal.Id, cancellationToken).ConfigureAwait(false);
        return granted
            ? AuthorizationResult.Allow()
            : AuthorizationResult.Deny(Error.Denied(SecurityErrors.PermissionMissing, SecurityMessages.PermissionMissing, MessageArguments.Of(("permission", PermissionCode))));
    }

    /// <summary>
    /// 在临时作用域里判定
    /// </summary>
    private async ValueTask<bool> CheckInNewScopeAsync(string userId, CancellationToken cancellationToken)
    {
        await using var scope = _rootServices.CreateAsyncScope();
        return await CheckAsync(scope.ServiceProvider, userId, null, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 用给定作用域的应用检查器判定
    /// </summary>
    private async ValueTask<bool> CheckAsync(IServiceProvider services, string userId, HttpContext? httpContext, CancellationToken cancellationToken)
    {
        var checker = services.GetRequiredService<IPermissionChecker>();
        var result = await checker.CheckAsync(new PermissionCheckContext(userId, PermissionCode, resource: httpContext), cancellationToken).ConfigureAwait(false);
        return result.IsGranted;
    }
}
