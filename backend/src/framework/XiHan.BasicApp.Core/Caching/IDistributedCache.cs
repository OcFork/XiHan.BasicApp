// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Caching.Distributed;

namespace XiHan.BasicApp.Core.Caching;

/// <summary>
/// 按缓存项类型分区的分布式缓存（承接 4.x 契约的异步子集，架在 5.0 <c>ISharedCache</c> 上）
/// </summary>
/// <remarks>
/// 键按 <see cref="IDistributedCacheKeyNormalizer"/> 规范化（租户段 + 缓存名 + 业务键）。
/// <c>hideErrors</c> 为空时取 <see cref="DistributedCacheOptions.HideErrors"/>：为真时缓存故障只记日志并当作未命中；
/// <c>considerUow</c> 仅为源码兼容保留，5.0 下缓存不参与工作单元。
/// </remarks>
/// <typeparam name="TCacheItem">缓存项类型</typeparam>
/// <typeparam name="TCacheKey">业务键类型</typeparam>
public interface IDistributedCache<TCacheItem, TCacheKey>
    where TCacheItem : class
{
    /// <summary>
    /// 读取；未命中返回空
    /// </summary>
    /// <param name="key">业务键</param>
    /// <param name="hideErrors">是否吞掉缓存故障</param>
    /// <param name="considerUow">兼容保留，忽略</param>
    /// <param name="token">取消令牌</param>
    Task<TCacheItem?> GetAsync(TCacheKey key, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default);

    /// <summary>
    /// 读取，未命中时用工厂生成并写入
    /// </summary>
    /// <param name="key">业务键</param>
    /// <param name="factory">生成缓存项</param>
    /// <param name="optionsFactory">生成过期策略；为空用默认</param>
    /// <param name="hideErrors">是否吞掉缓存故障</param>
    /// <param name="considerUow">兼容保留，忽略</param>
    /// <param name="token">取消令牌</param>
    Task<TCacheItem?> GetOrAddAsync(TCacheKey key, Func<Task<TCacheItem>> factory, Func<DistributedCacheEntryOptions>? optionsFactory = null, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default);

    /// <summary>
    /// 写入
    /// </summary>
    /// <param name="key">业务键</param>
    /// <param name="value">缓存项</param>
    /// <param name="options">过期策略；为空用默认</param>
    /// <param name="hideErrors">是否吞掉缓存故障</param>
    /// <param name="considerUow">兼容保留，忽略</param>
    /// <param name="token">取消令牌</param>
    Task SetAsync(TCacheKey key, TCacheItem value, DistributedCacheEntryOptions? options = null, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default);

    /// <summary>
    /// 批量写入
    /// </summary>
    /// <param name="items">键值对</param>
    /// <param name="options">过期策略；为空用默认</param>
    /// <param name="hideErrors">是否吞掉缓存故障</param>
    /// <param name="considerUow">兼容保留，忽略</param>
    /// <param name="token">取消令牌</param>
    Task SetManyAsync(IEnumerable<KeyValuePair<TCacheKey, TCacheItem>> items, DistributedCacheEntryOptions? options = null, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default);

    /// <summary>
    /// 续活滑动过期（5.0 下等价于一次读取）
    /// </summary>
    /// <param name="key">业务键</param>
    /// <param name="hideErrors">是否吞掉缓存故障</param>
    /// <param name="token">取消令牌</param>
    Task RefreshAsync(TCacheKey key, bool? hideErrors = null, CancellationToken token = default);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="key">业务键</param>
    /// <param name="hideErrors">是否吞掉缓存故障</param>
    /// <param name="considerUow">兼容保留，忽略</param>
    /// <param name="token">取消令牌</param>
    Task RemoveAsync(TCacheKey key, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default);

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="keys">业务键</param>
    /// <param name="hideErrors">是否吞掉缓存故障</param>
    /// <param name="considerUow">兼容保留，忽略</param>
    /// <param name="token">取消令牌</param>
    Task RemoveManyAsync(IEnumerable<TCacheKey> keys, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default);

    /// <summary>
    /// 是否存在
    /// </summary>
    /// <param name="key">业务键</param>
    /// <param name="hideErrors">是否吞掉缓存故障</param>
    /// <param name="considerUow">兼容保留，忽略</param>
    /// <param name="token">取消令牌</param>
    Task<bool> ExistsAsync(TCacheKey key, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default);

    /// <summary>
    /// 按业务键通配模式（<c>*</c>）删除本缓存分区内的键；需要注册 <see cref="ICacheKeyPatternRemover"/>
    /// </summary>
    /// <param name="pattern">业务键模式，默认全部</param>
    /// <param name="hideErrors">是否吞掉缓存故障</param>
    /// <param name="considerUow">兼容保留，忽略</param>
    /// <param name="token">取消令牌</param>
    /// <returns>删除的键数</returns>
    Task<long> RemoveByPatternAsync(string pattern = "*", bool? hideErrors = null, bool considerUow = false, CancellationToken token = default);
}
