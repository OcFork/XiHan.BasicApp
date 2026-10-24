// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace XiHan.BasicApp.Core.Caching;

/// <summary>
/// 应用分布式缓存注册
/// </summary>
public static class CachingServiceCollectionExtensions
{
    /// <summary>
    /// Redis 配置节（与 5.0 <c>RedisCachingFeature</c> 同一节：<c>XiHan:Caching:Redis</c>）
    /// </summary>
    public const string RedisSectionName = "XiHan:Caching:Redis";

    /// <summary>
    /// 注册按缓存项类型分区的 <see cref="IDistributedCache{TCacheItem, TCacheKey}"/>（架在 5.0 共享缓存上）、资源锁，
    /// 以及承接代码直接消费的 Microsoft <c>IDistributedCache</c>（启用 Redis 时复用 5.0 缓存特性建立的连接，否则进程内内存）
    /// </summary>
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

        // Microsoft IDistributedCache：一次性验证码、刷新令牌、访问策略快照等承接代码直接消费它；
        // 与 4.x 一样按 Redis 开关选实现，Redis 下不另开连接，复用 5.0 RedisCachingFeature 注册的 IConnectionMultiplexer
        var redis = configuration.GetSection(RedisSectionName);
        if (redis.GetValue<bool>("IsEnabled"))
        {
            services.AddStackExchangeRedisCache(static _ => { });
            services.AddOptions<RedisCacheOptions>()
                .Configure<IServiceProvider>((options, provider) =>
                {
                    options.InstanceName = redis["InstanceName"];
                    options.ConnectionMultiplexerFactory = () => Task.FromResult(provider.GetRequiredService<IConnectionMultiplexer>());
                });
        }
        else
        {
            services.AddDistributedMemoryCache();
        }

        return services;
    }
}
