// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using XiHan.BasicApp.Saas.Domain.Permissions;
using XiHan.BasicApp.Saas.Infrastructure.Seeders;
using XiHan.Framework.Kernel.Hosting;
using XiHan.Framework.Kernel.Security;

namespace XiHan.BasicApp.Saas.Infrastructure.Security;

/// <summary>
/// 把应用的权限目录接进 5.0 授权：每个权限码注册一个同名策略 <see cref="AppPermissionPolicy"/>（<c>permission:&lt;code&gt;</c>）。
/// 操作契约上的 <c>Permission</c> 只有找得到同名策略才会放行判定，否则授权中间件按「策略不存在」拒绝
/// </summary>
/// <remarks>
/// 同一权限码在不同模块重复声明会让策略目录在启动时抛「策略名重复」——这是真实的目录冲突，故意不吞掉。
/// </remarks>
public static class PermissionRegistrationExtensions
{
    /// <summary>
    /// 注册 SaaS 权限定义（<see cref="SaasPermissionDefinitions.All"/>）对应的策略
    /// </summary>
    /// <param name="context">特性上下文</param>
    /// <param name="definitions">权限定义</param>
    /// <returns>特性上下文</returns>
    public static FeatureContext AddPermissionDefinitions(this FeatureContext context, IEnumerable<SaasPermissionDefinition> definitions)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(definitions);
        AddPolicies(context, definitions.Select(definition => definition.PermissionCode));
        return context;
    }

    /// <summary>
    /// 注册权限目录种子（各模块 <c>PermissionCatalogSeeder</c> 的静态种子）对应的策略
    /// </summary>
    /// <param name="context">特性上下文</param>
    /// <param name="seeds">权限种子</param>
    /// <returns>特性上下文</returns>
    public static FeatureContext AddPermissionSeeds(this FeatureContext context, IEnumerable<PermissionSeed> seeds)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(seeds);
        AddPolicies(context, seeds.Select(seed => seed.Code));
        return context;
    }

    /// <summary>
    /// 按权限码注册策略；同一清单里重复出现的码只注册一次
    /// </summary>
    private static void AddPolicies(FeatureContext context, IEnumerable<string> codes)
    {
        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var code in codes)
        {
            if (string.IsNullOrWhiteSpace(code) || !seen.Add(code))
            {
                continue;
            }

            context.Services.AddSingleton<IPolicy>(services => new AppPermissionPolicy(code, services));
        }
    }
}
