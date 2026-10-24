// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using XiHan.BasicApp.Core.Caching;
using XiHan.Framework.Caching.Redis;

namespace XiHan.BasicApp.Saas.Infrastructure.Caching;

/// <summary>
/// 按键模式删除的 Redis 实现：在 5.0 Redis 缓存存储的键前缀下 SCAN + DEL（5.0 缓存存储没有模式删除，先由应用顶着）
/// </summary>
/// <param name="services">服务提供者：连接与前缀选项只在启用 Redis 缓存时才注册，所以按需解析</param>
// xihan-migrate(P5-09): 框架补上按前缀/模式删除后，这里改为直接调用框架能力。
public sealed class RedisCacheKeyPatternRemover(IServiceProvider services) : ICacheKeyPatternRemover
{
    /// <summary>
    /// 每批扫描与删除的键数
    /// </summary>
    private const int BatchSize = 512;

    /// <inheritdoc />
    public async ValueTask<long> RemoveByPatternAsync(string pattern, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(pattern);
        var multiplexer = services.GetService<IConnectionMultiplexer>()
                          ?? throw new NotSupportedException("Removing cache keys by pattern requires the Redis caching provider; enable 'XiHan:Caching:Redis'.");
        var prefix = services.GetService<RedisCachingOptions>()?.KeyPrefix ?? string.Empty;
        var fullPattern = prefix + pattern;
        var database = multiplexer.GetDatabase();
        long removed = 0;
        foreach (var endpoint in multiplexer.GetEndPoints())
        {
            var server = multiplexer.GetServer(endpoint);
            // 只在主节点上扫描，副本上的键与主节点一致
            if (!server.IsConnected || server.IsReplica)
            {
                continue;
            }

            var batch = new List<RedisKey>(BatchSize);
            await foreach (var key in server.KeysAsync(database.Database, fullPattern, BatchSize).WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                batch.Add(key);
                if (batch.Count == BatchSize)
                {
                    removed += await database.KeyDeleteAsync([.. batch]).ConfigureAwait(false);
                    batch.Clear();
                }
            }

            if (batch.Count > 0)
            {
                removed += await database.KeyDeleteAsync([.. batch]).ConfigureAwait(false);
            }
        }

        return removed;
    }
}
