// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Saas;
using XiHan.BasicApp.Workflow.Extensions;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Hosting;
using XiHan.Framework.Primitives;
using XiHan.Framework.Primitives.Manifests;

namespace XiHan.BasicApp.Workflow;

/// <summary>
/// 曦寒基础应用工作流应用模块
/// </summary>
/// <remarks>
/// 在框架工作流引擎（XiHanWorkflowModule）之上提供：SqlSugar 持久化存储（Replace 内存默认）、
/// 定义/实例/待办应用服务、权限与菜单种子、待办站内通知。
/// </remarks>
// xihan-migrate(V4-01): 已删除没有 5.0 能力的 4.x 模块：XiHanWorkflowModule。
public sealed class BasicAppWorkflowFeature : IFeature
{
    /// <inheritdoc />
    public Manifest Manifest { get; } = new()
    {
        Id = "xihan.basicapp.workflow",
        Package = "XiHan.BasicApp.Workflow",
        Version = "1.0.0",
        Primitives = 1,
        Stability = Stability.Stable,
        Kind = ManifestKind.Capability,
        Requires =
        [
            new CapabilityRequirement(KernelCapabilities.Hosting, KernelCapabilities.Version),
        ],
        Entry = typeof(BasicAppWorkflowFeature).FullName,
    };

    /// <inheritdoc />
    public void Configure(FeatureContext context)
    {
        var services = context.Services;

        // 仓储与应用服务由框架约定扫描自动注册；此处登记存储替换、受理人解析器、审批业务接入、种子与事件处理器
        services.AddWorkflowStores();
        services.AddWorkflowAssigneeProviders();
        services.AddWorkflowBusinessIntegration();
        services.AddWorkflowFieldSecurityEntities();
        services.AddWorkflowDataSeeders();
        services.AddWorkflowDemoData();
        services.AddWorkflowEventHandlers();
    }
}
