// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace XiHan.BasicApp.Core.Security;

/// <summary>
/// 承接 4.x Security 模块的密码相关注册
/// </summary>
public static class SecurityServiceCollectionExtensions
{
    /// <summary>
    /// 绑定密码哈希与密码策略选项（<c>XiHan:Authentication:PasswordHasher</c> / <c>XiHan:Authentication:PasswordPolicy</c>），注册密码哈希器；
    /// 密码历史存储由应用（Saas）提供
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddXiHanSecurityServices(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        services.Configure<PasswordHasherOptions>(configuration.GetSection(PasswordHasherOptions.SectionName));
        services.Configure<PasswordPolicyOptions>(configuration.GetSection(PasswordPolicyOptions.SectionName));
        services.TryAddSingleton<IPasswordHasher, PasswordHasher>();
        return services;
    }
}
