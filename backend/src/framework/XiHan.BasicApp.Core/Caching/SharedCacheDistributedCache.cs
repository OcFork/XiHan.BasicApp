// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XiHan.BasicApp.Core.Tenancy;
using XiHan.Framework.Caching.Abstractions;

namespace XiHan.BasicApp.Core.Caching;

/// <summary>
/// 把 4.x 形态的 <see cref="IDistributedCache{TCacheItem, TCacheKey}"/> 架在 5.0 的 <see cref="ISharedCache"/> 上
/// </summary>
/// <typeparam name="TCacheItem">缓存项类型</typeparam>
/// <typeparam name="TCacheKey">业务键类型</typeparam>
public sealed class SharedCacheDistributedCache<TCacheItem, TCacheKey> : IDistributedCache<TCacheItem, TCacheKey>
    where TCacheItem : class
{
    private readonly ISharedCache _cache;
    private readonly IDistributedCacheKeyNormalizer _normalizer;
    private readonly DistributedCacheOptions _options;
    private readonly ILogger<SharedCacheDistributedCache<TCacheItem, TCacheKey>> _logger;
    private readonly ICacheKeyPatternRemover? _patternRemover;

    /// <summary>
    /// 缓存名：取 <see cref="CacheNameAttribute"/>，没有则按类型全名
    /// </summary>
    private static readonly string CacheName = CacheNameAttribute.GetCacheName<TCacheItem>();

    /// <summary>
    /// 缓存项是否声明了忽略多租户（键不带租户段）
    /// </summary>
    private static readonly bool IgnoreMultiTenancy = typeof(TCacheItem).IsDefined(typeof(IgnoreMultiTenancyAttribute), true);

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="cache">5.0 跨实例共享缓存</param>
    /// <param name="normalizer">键规范化</param>
    /// <param name="options">选项</param>
    /// <param name="logger">日志</param>
    /// <param name="patternRemover">按模式删除；未注册时 <see cref="RemoveByPatternAsync"/> 抛出</param>
    public SharedCacheDistributedCache(
        ISharedCache cache,
        IDistributedCacheKeyNormalizer normalizer,
        IOptions<DistributedCacheOptions> options,
        ILogger<SharedCacheDistributedCache<TCacheItem, TCacheKey>> logger,
        ICacheKeyPatternRemover? patternRemover = null)
    {
        _cache = cache;
        _normalizer = normalizer;
        _options = options.Value;
        _logger = logger;
        _patternRemover = patternRemover;
    }

    /// <inheritdoc />
    public Task<TCacheItem?> GetAsync(TCacheKey key, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default)
        => GuardAsync(hideErrors, async () =>
        {
            var value = await _cache.GetAsync<TCacheItem>(NormalizeKey(key), token).ConfigureAwait(false);
            return value.TryGetValue(out var item) ? item : null;
        }, null);

    /// <inheritdoc />
    public Task<TCacheItem?> GetOrAddAsync(TCacheKey key, Func<Task<TCacheItem>> factory, Func<DistributedCacheEntryOptions>? optionsFactory = null, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(factory);
        return Core();

        async Task<TCacheItem?> Core()
        {
            var cacheKey = NormalizeKey(key);
            var cached = await GuardAsync(hideErrors, async () =>
            {
                var value = await _cache.GetAsync<TCacheItem>(cacheKey, token).ConfigureAwait(false);
                return value.TryGetValue(out var item) ? item : null;
            }, null).ConfigureAwait(false);
            if (cached is not null)
            {
                return cached;
            }

            // 工厂的异常不属于缓存故障，原样抛出
            var created = await factory().ConfigureAwait(false);
            await GuardAsync(hideErrors, async () =>
            {
                await _cache.SetAsync(cacheKey, created, ToEntryOptions(optionsFactory?.Invoke()), token).ConfigureAwait(false);
                return true;
            }, false).ConfigureAwait(false);
            return created;
        }
    }

    /// <inheritdoc />
    public Task SetAsync(TCacheKey key, TCacheItem value, DistributedCacheEntryOptions? options = null, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(value);
        return GuardAsync(hideErrors, async () =>
        {
            await _cache.SetAsync(NormalizeKey(key), value, ToEntryOptions(options), token).ConfigureAwait(false);
            return true;
        }, false);
    }

    /// <inheritdoc />
    public Task SetManyAsync(IEnumerable<KeyValuePair<TCacheKey, TCacheItem>> items, DistributedCacheEntryOptions? options = null, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(items);
        return GuardAsync(hideErrors, async () =>
        {
            var entries = items.Select(pair => new KeyValuePair<CacheKey, TCacheItem>(NormalizeKey(pair.Key), pair.Value)).ToList();
            if (entries.Count > 0)
            {
                await _cache.SetManyAsync(entries, ToEntryOptions(options), token).ConfigureAwait(false);
            }

            return true;
        }, false);
    }

    /// <inheritdoc />
    public Task RefreshAsync(TCacheKey key, bool? hideErrors = null, CancellationToken token = default)
        => GuardAsync(hideErrors, async () =>
        {
            // 5.0 没有单独的续活：读取一次即触发滑动过期的续活
            await _cache.GetAsync<TCacheItem>(NormalizeKey(key), token).ConfigureAwait(false);
            return true;
        }, false);

    /// <inheritdoc />
    public Task RemoveAsync(TCacheKey key, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default)
        => GuardAsync(hideErrors, async () => await _cache.RemoveAsync(NormalizeKey(key), token).ConfigureAwait(false), false);

    /// <inheritdoc />
    public Task RemoveManyAsync(IEnumerable<TCacheKey> keys, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(keys);
        return GuardAsync(hideErrors, async () =>
        {
            var cacheKeys = keys.Select(NormalizeKey).ToList();
            if (cacheKeys.Count > 0)
            {
                await _cache.RemoveManyAsync(cacheKeys, token).ConfigureAwait(false);
            }

            return true;
        }, false);
    }

    /// <inheritdoc />
    public Task<bool> ExistsAsync(TCacheKey key, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default)
        => GuardAsync(hideErrors, async () => (await _cache.GetAsync<TCacheItem>(NormalizeKey(key), token).ConfigureAwait(false)).HasValue, false);

    /// <inheritdoc />
    public Task<long> RemoveByPatternAsync(string pattern = "*", bool? hideErrors = null, bool considerUow = false, CancellationToken token = default)
    {
        // 没有实现是装配错误，不按缓存故障吞掉
        if (_patternRemover is null)
        {
            throw new NotSupportedException($"Removing cache keys by pattern requires an '{nameof(ICacheKeyPatternRemover)}' registration; none is registered.");
        }

        var businessPattern = string.IsNullOrWhiteSpace(pattern) ? "*" : pattern.Trim();
        var fullPattern = _normalizer.NormalizeKey(new DistributedCacheKeyNormalizeArgs(businessPattern, CacheName, IgnoreMultiTenancy));
        return GuardAsync(hideErrors, async () => await _patternRemover.RemoveByPatternAsync(fullPattern, token).ConfigureAwait(false), 0L);
    }

    /// <summary>
    /// 业务键 → 规范化缓存键
    /// </summary>
    private CacheKey NormalizeKey(TCacheKey key)
    {
        ArgumentNullException.ThrowIfNull(key);
        var text = key.ToString();
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("The cache key must not be empty.", nameof(key));
        }

        return new CacheKey(_normalizer.NormalizeKey(new DistributedCacheKeyNormalizeArgs(text, CacheName, IgnoreMultiTenancy)));
    }

    /// <summary>
    /// 4.x 过期策略 → 5.0；没给或没设任何过期时用默认
    /// </summary>
    private CacheEntryOptions ToEntryOptions(DistributedCacheEntryOptions? options)
    {
        if (options is null)
        {
            return _options.DefaultEntryOptions;
        }

        var timeToLive = options.AbsoluteExpirationRelativeToNow
                         ?? (options.AbsoluteExpiration is { } absolute ? absolute - DateTimeOffset.UtcNow : null);
        if (timeToLive is null && options.SlidingExpiration is null)
        {
            return _options.DefaultEntryOptions;
        }

        if (timeToLive is { } ttl && ttl <= TimeSpan.Zero)
        {
            // 已过期的绝对时间：写入即失效，给最短存活
            timeToLive = TimeSpan.FromMilliseconds(1);
        }

        return new CacheEntryOptions { TimeToLive = timeToLive, SlidingExpiration = options.SlidingExpiration };
    }

    /// <summary>
    /// 缓存故障处理：hideErrors 为真时记日志并返回回退值，否则抛出；取消与装配错误始终抛出
    /// </summary>
    private async Task<T> GuardAsync<T>(bool? hideErrors, Func<Task<T>> action, T fallback)
    {
        try
        {
            return await action().ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not OperationCanceledException and not NotSupportedException and not ArgumentException && (hideErrors ?? _options.HideErrors))
        {
            _logger.LogWarning(ex, "Cache '{CacheName}' operation failed; treated as a miss.", CacheName);
            return fallback;
        }
    }
}
