// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.CodeGeneration.Extensions;

using XiHan.BasicApp.Saas;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Hosting;
using XiHan.Framework.Primitives;
using XiHan.Framework.Primitives.Manifests;
using XiHan.BasicApp.CodeGeneration.Infrastructure.Seeders;
using XiHan.BasicApp.Saas.Infrastructure.Security;

namespace XiHan.BasicApp.CodeGeneration;

/// <summary>
/// 曦寒基础应用代码生成应用模块
/// </summary>
public sealed class BasicAppCodeGenerationFeature : IFeature
{
    /// <inheritdoc />
    public Manifest Manifest { get; } = new()
    {
        Id = "xihan.basicapp.codegeneration",
        Package = "XiHan.BasicApp.CodeGeneration",
        Version = "1.0.0",
        Primitives = 1,
        Stability = Stability.Stable,
        Kind = ManifestKind.Capability,
        Requires =
        [
            new CapabilityRequirement(KernelCapabilities.Hosting, KernelCapabilities.Version),
        ],
        Entry = typeof(BasicAppCodeGenerationFeature).FullName,
    };

    /// <inheritdoc />
    public void Configure(FeatureContext context)
    {
        var services = context.Services;

        // 登记本程序集全部 [Operation]（源生成器产出）与本模块权限目录
        context.AddGeneratedOperations();
        context.AddPermissionSeeds(CodeGenPermissionCatalogSeeder.Seeds);

        // 注册代码生成模块种子数据
        services.AddCodeGenerationDataSeeders();

        // 注册代码生成引擎（渲染器/类型映射/扫描/打包/编排）
        services.AddCodeGenerationEngine();

        // 注册代码生成领域服务（未携带 DI 标记接口，需显式登记）
        services.AddCodeGenerationDomainServices();
        services.AddCodeGenerationFieldSecurityEntities();
    }
}
