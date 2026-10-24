// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using XiHan.BasicApp.Printing.Domain.DataSources;
using XiHan.BasicApp.Printing.Extensions;
using XiHan.BasicApp.Saas;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Hosting;
using XiHan.Framework.Primitives;
using XiHan.Framework.Primitives.Manifests;
using XiHan.BasicApp.Printing.Infrastructure.Seeders;
using XiHan.BasicApp.Saas.Infrastructure.Security;

namespace XiHan.BasicApp.Printing;

/// <summary>
/// 曦寒基础应用打印应用模块
/// </summary>
/// <remarks>
/// 独立一等模块（与 AI/代码生成/工作流同构）：打印模板的租户/平台双作用域管理、
/// 按编码解析与分布式缓存，消费方为业务模块的单据打印与前端打印设计器。
/// 依赖 Saas（复用 RBAC 表、SaasRepository、菜单/权限种子基类）。
/// </remarks>
public sealed class BasicAppPrintingFeature : IHostedFeature
{
    /// <inheritdoc />
    public Manifest Manifest { get; } = new()
    {
        Id = "xihan.basicapp.printing",
        Package = "XiHan.BasicApp.Printing",
        Version = "1.0.0",
        Primitives = 1,
        Stability = Stability.Stable,
        Kind = ManifestKind.Capability,
        Requires =
        [
            new CapabilityRequirement(KernelCapabilities.Hosting, KernelCapabilities.Version),
        ],
        Entry = typeof(BasicAppPrintingFeature).FullName,
    };

    /// <inheritdoc />
    public void Configure(FeatureContext context)
    {
        var services = context.Services;

        // 登记本程序集全部 [Operation]（源生成器产出）与本模块权限目录
        context.AddGeneratedOperations();
        context.AddPermissionSeeds(PrintingPermissionCatalogSeeder.Seeds);

        // 种子（权限 → 菜单 → 角色授权）+ 领域服务 + 解析器与缓存 + 数据源注册表（含内置示例）
        services.AddPrintingDataSeeders();
        services.AddPrintingDomainServices();
        services.AddPrintingApplicationServices();
        services.AddPrintingFieldSecurityEntities();
        services.AddPrintingDataSources();
        services.AddPrintingDemoData(context.Configuration);
    }

    /// <inheritdoc />
    public Task StartAsync(IServiceProvider services, CancellationToken cancellationToken)
    {
        _ = services.GetRequiredService<IPrintDataSourceRegistry>();

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
