// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection.Extensions;
using XiHan.BasicApp.Core;
using XiHan.BasicApp.Web.Core.Upgrade;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Hosting;
using XiHan.Framework.Primitives;
using XiHan.Framework.Primitives.Manifests;
using XiHan.Framework.Protocols.Http;
using XiHan.Framework.Protocols.Mcp;
using XiHan.BasicApp.Core.Upgrade.Abstractions;

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

        // 维护模式：升级引擎置位、中间件据此拦截业务请求；中间件经启动过滤器挂到管线最前
        services.TryAddSingleton<MaintenanceModeState>();
        services.Replace(ServiceDescriptor.Singleton<IUpgradeMaintenanceModeManager, BasicAppUpgradeMaintenanceModeManager>());
        services.AddTransient<IStartupFilter, MaintenanceModeStartupFilter>();
    }
}
