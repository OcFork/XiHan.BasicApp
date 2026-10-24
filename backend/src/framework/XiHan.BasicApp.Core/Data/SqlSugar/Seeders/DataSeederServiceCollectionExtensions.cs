// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

namespace XiHan.BasicApp.Core.Data.SqlSugar.Seeders;

/// <summary>
/// 数据种子注册（承接 4.x）
/// </summary>
public static class DataSeederServiceCollectionExtensions
{
    /// <summary>
    /// 注册一个数据种子（作用域生命周期，初始化器按注册顺序执行）
    /// </summary>
    /// <typeparam name="TSeeder">种子类型</typeparam>
    /// <param name="services">服务集合</param>
    public static IServiceCollection AddDataSeeder<TSeeder>(this IServiceCollection services)
        where TSeeder : class, IDataSeeder
    {
        ArgumentNullException.ThrowIfNull(services);
        services.AddScoped<IDataSeeder, TSeeder>();
        return services;
    }

    /// <summary>
    /// 批量注册数据种子
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="seederTypes">种子类型；必须实现 <see cref="IDataSeeder"/></param>
    public static IServiceCollection AddDataSeeders(this IServiceCollection services, params Type[] seederTypes)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(seederTypes);
        foreach (var seederType in seederTypes)
        {
            if (!typeof(IDataSeeder).IsAssignableFrom(seederType))
            {
                throw new ArgumentException($"Type '{seederType.Name}' must implement '{nameof(IDataSeeder)}'.", nameof(seederTypes));
            }

            services.AddScoped(typeof(IDataSeeder), seederType);
        }

        return services;
    }
}
