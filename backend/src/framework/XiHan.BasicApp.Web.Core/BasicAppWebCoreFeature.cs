// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection.Extensions;
using XiHan.BasicApp.Core.Upgrade.Abstractions;
using XiHan.BasicApp.Core;
using XiHan.BasicApp.Web.Core.Authentication.Extensions.DependencyInjection;
using XiHan.BasicApp.Web.Core.Authentication.Oidc;
using XiHan.BasicApp.Web.Core.Upgrade;
using XiHan.Framework.Kernel.Hosting;
using XiHan.Framework.Kernel;
using XiHan.Framework.Primitives.Manifests;
using XiHan.Framework.Primitives;
using XiHan.Framework.Protocols.Http;
using XiHan.Framework.Protocols.Mcp;
using XiHan.BasicApp.Web.Core.Contexts;
using Microsoft.Extensions.Configuration;
using XiHan.BasicApp.Web.Core.Clients;
using XiHan.BasicApp.Web.Core.RealTime.Extensions;
using XiHan.BasicApp.Core.Entities;

namespace XiHan.BasicApp.Web.Core;

/// <summary>
/// XiHanBasicAppWebCoreModule
/// </summary>
// xihan-migrate(V4-01): 已删除没有 5.0 能力的 4.x 模块：XiHanWebRealTimeModule, XiHanWebGatewayModule。
public sealed class BasicAppWebCoreFeature : IFeature
{
    /// <inheritdoc />
    public Manifest Manifest { get; } = new()
    {
        Id = "xihan.basicapp.web-core",
        Package = "XiHan.BasicApp.Web.Core",
        Version = "1.0.0",
        Primitives = 1,
        Stability = Stability.Stable,
        Kind = ManifestKind.Capability,
        Requires =
        [
            new CapabilityRequirement(KernelCapabilities.Hosting, KernelCapabilities.Version),
            new CapabilityRequirement(HttpProtocolCapabilities.Http, HttpProtocolCapabilities.Version),
            new CapabilityRequirement(McpProtocolCapabilities.Mcp, McpProtocolCapabilities.Version),
        ],
        Entry = typeof(BasicAppWebCoreFeature).FullName,
    };

    /// <inheritdoc />
    public void Configure(FeatureContext context)
    {
        var services = context.Services;

        // 4.x 的 Web 模块负责注册 IHttpContextAccessor；5.0 宿主不注册它，而权限检查器、客户端信息、访问日志等移植代码都依赖它
        services.AddHttpContextAccessor();
        services.TryAddSingleton<IRequestContextAccessor, RequestContextAccessor>();
        services.TryAddScoped<ITraceIdProvider, HttpTraceIdProvider>();
        // 客户端信息与 IP 归属地（登录地址、访问策略的地区条件共用一份离线库；原 4.x Web.Core 模块）
        services.Configure<XiHanClientInfoOptions>(context.Configuration.GetSection(XiHanClientInfoOptions.SectionName));
        services.TryAddSingleton<IIpRegionResolver, Ip2RegionResolver>();
        services.TryAddSingleton<IClientInfoProvider, HttpContextClientInfoProvider>();
        // SignalR：连接管理器、用户标识提供器、按 Hub 的实时通知服务（原 4.x SignalR 模块）
        services.AddXiHanSignalRWithJson(context.Configuration);

        // 维护模式：升级引擎置位、中间件据此拦截业务请求；中间件经启动过滤器挂到管线最前
        services.TryAddSingleton<MaintenanceModeState>();
        services.Replace(ServiceDescriptor.Singleton<IUpgradeMaintenanceModeManager, BasicAppUpgradeMaintenanceModeManager>());
        services.AddTransient<IStartupFilter, MaintenanceModeStartupFilter>();

        // 承接的 4.x 认证（JWT 签发、OTP、一次性验证码、第三方登录）与 OIDC 签发（XiHan:Authentication:Oidc:IsEnabled 门控）
        // ASP.NET Core 自身的认证：Hub 的 [Authorize]、OIDC UserInfo、第三方登录回调仍走它；与 5.0 管道共用同一把 JWT 密钥，不设回退策略
        services.AddXiHanAspNetAuthentication(context.Configuration);
        services.AddXiHanAuthentication(context.Configuration);
        services.AddXiHanOidc(context.Configuration);
    }
}
