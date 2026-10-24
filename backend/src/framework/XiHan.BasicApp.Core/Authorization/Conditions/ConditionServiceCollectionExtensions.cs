// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace XiHan.BasicApp.Core.Authorization.Conditions;

/// <summary>
/// 权限条件注册（承接 4.x 授权模块的条件部分）
/// </summary>
public static class ConditionServiceCollectionExtensions
{
    /// <summary>
    /// 注册条件属性登记表、条件编译器与条件求值器（登记项经 <see cref="AddXiHanConditionAttributes"/> 追加）
    /// </summary>
    /// <param name="services">服务集合</param>
    public static IServiceCollection AddBasicAppConditions(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.AddOptions<XiHanConditionAttributeOptions>();
        // 登记表在首次解析时一次建好，重名即抛出
        services.TryAddSingleton<IConditionAttributeRegistry>(provider => new DefaultConditionAttributeRegistry(provider.GetRequiredService<IOptions<XiHanConditionAttributeOptions>>()));
        services.TryAddSingleton<IConditionCompiler, DefaultConditionCompiler>();
        services.TryAddSingleton<IConditionEvaluator, DefaultConditionEvaluator>();
        return services;
    }

    /// <summary>
    /// 追加条件属性登记项（可多次调用）
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="attributes">登记项</param>
    public static IServiceCollection AddXiHanConditionAttributes(this IServiceCollection services, params ConditionAttributeDescriptor[] attributes)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(attributes);
        services.AddBasicAppConditions();
        services.Configure<XiHanConditionAttributeOptions>(options => options.Attributes.AddRange(attributes));
        return services;
    }
}
