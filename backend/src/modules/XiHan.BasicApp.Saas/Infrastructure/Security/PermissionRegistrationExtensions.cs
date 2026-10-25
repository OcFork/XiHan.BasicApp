// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Infrastructure.Seeders;
using XiHan.Framework.Kernel.Hosting;
using XiHan.Framework.Kernel.Security;

namespace XiHan.BasicApp.Saas.Infrastructure.Security;

/// <summary>
/// 把应用的权限目录登记进内核 <c>PermissionRegistry</c>：操作契约上的 <c>Permission</c> 只有登记过的权限码才会生成
/// <c>permission:&lt;code&gt;</c> 策略，未登记的按「策略不存在」拒绝。权限码遵循内核文法（小写点分，段内只有字母、数字、连字符）
/// </summary>
public static class PermissionRegistrationExtensions
{
    /// <summary>
    /// 登记 SaaS 权限定义（<see cref="SaasPermissionDefinitions.All"/>）
    /// </summary>
    /// <param name="context">特性上下文</param>
    /// <param name="definitions">权限定义</param>
    /// <returns>特性上下文</returns>
    public static FeatureContext AddPermissionDefinitions(this FeatureContext context, IEnumerable<SaasPermissionDefinition> definitions)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(definitions);
        foreach (var definition in definitions)
        {
            Add(context, definition.PermissionCode, definition.PermissionName, definition.PermissionDescription);
        }

        return context;
    }

    /// <summary>
    /// 登记权限目录种子（各模块 <c>PermissionCatalogSeeder</c> 的静态种子）
    /// </summary>
    /// <param name="context">特性上下文</param>
    /// <param name="seeds">权限种子</param>
    /// <returns>特性上下文</returns>
    public static FeatureContext AddPermissionSeeds(this FeatureContext context, IEnumerable<PermissionSeed> seeds)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(seeds);
        foreach (var seed in seeds)
        {
            Add(context, seed.Code, seed.Name, seed.Description);
        }

        return context;
    }

    /// <summary>
    /// 登记一条权限；同一权限码被多个目录声明时以先登记的为准，不让重复定义把宿主启动打断
    /// </summary>
    private static void Add(FeatureContext context, string code, string name, string description)
    {
        if (context.Permissions.Find(code).HasValue)
        {
            return;
        }

        _ = context.AddPermission(new PermissionDefinition(code, name, string.IsNullOrWhiteSpace(description) ? null : description));
    }
}
