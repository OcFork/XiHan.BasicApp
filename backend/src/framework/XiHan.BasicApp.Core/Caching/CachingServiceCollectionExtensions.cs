// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Configuration;
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
    /// <remarks>
    /// 承接代码直接消费的 Microsoft <c>IDistributedCache</c> 不在这里登记：5.0 的 <c>caching.distributed</c> Provider
    /// （画像里的内存缓存，或开关打开时的 <c>RedisCachingFeature</c>）被选中时登记 <c>CacheStoreDistributedCache</c>，
    /// 它架在同一个 <c>ICacheStore</c> 上，键须满足 <c>CacheKey</c> 文法（无空白与控制字符、不超过 512 字符）。
    /// </remarks>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置</param>
    /// <param name="configure">选项</param>
    public static IServiceCollection AddBasicAppCaching(this IServiceCollection services, IConfiguration configuration, Action<DistributedCacheOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var builder = services.AddOptions<DistributedCacheOptions>();
        if (configure is not null)
        {
            builder.Configure(configure);
        }

        // 键规范化从执行流环境读当前租户，不依赖作用域服务，所以分区缓存可以是单例（后台单例服务也在用它）
        services.TryAddSingleton<IDistributedCacheKeyNormalizer, DefaultDistributedCacheKeyNormalizer>();
        services.TryAddSingleton(typeof(IDistributedCache<,>), typeof(SharedCacheDistributedCache<,>));
        services.TryAddSingleton<IResourceLock, SharedResourceLock>();
        return services;
    }
}
