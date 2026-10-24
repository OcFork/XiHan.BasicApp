// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace XiHan.BasicApp.Core.Caching;

/// <summary>
/// 应用分布式缓存注册
/// </summary>
public static class CachingServiceCollectionExtensions
{
    /// <summary>
    /// 注册按缓存项类型分区的 <see cref="IDistributedCache{TCacheItem, TCacheKey}"/>（架在 5.0 共享缓存上）与资源锁
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configure">选项</param>
    public static IServiceCollection AddBasicAppCaching(this IServiceCollection services, Action<DistributedCacheOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        var builder = services.AddOptions<DistributedCacheOptions>();
        if (configure is not null)
        {
            builder.Configure(configure);
        }

        // 键规范化依赖当前租户（随调用作用域），缓存分区随之按作用域解析
        services.TryAddScoped<IDistributedCacheKeyNormalizer, DefaultDistributedCacheKeyNormalizer>();
        services.TryAddScoped(typeof(IDistributedCache<,>), typeof(SharedCacheDistributedCache<,>));
        services.TryAddSingleton<IResourceLock, SharedResourceLock>();
        return services;
    }
}
