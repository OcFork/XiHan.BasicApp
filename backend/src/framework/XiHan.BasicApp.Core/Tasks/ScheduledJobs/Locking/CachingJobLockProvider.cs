// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Core.Caching;
using XiHan.BasicApp.Core.Tasks.ScheduledJobs.Abstractions;

namespace XiHan.BasicApp.Core.Tasks.ScheduledJobs.Locking;

/// <summary>
/// 任务分布式锁提供者：适配到框架统一的分布式锁 <see cref="IResourceLock"/>（Caching 模块）。
/// </summary>
/// <remarks>
/// 不再自带 Redis/内存锁实现 —— 实际是 Redis 跨实例锁还是进程内回退，由 <see cref="IResourceLock"/>
/// 按 Redis 配置自动选择（启用 Redis → 跨实例；否则 → 进程内）。
/// </remarks>
public sealed class CachingJobLockProvider : IJobLockProvider
{
    private readonly IResourceLock _distributedLock;

    /// <summary>
    /// 构造函数
    /// </summary>
    public CachingJobLockProvider(IResourceLock distributedLock)
    {
        _distributedLock = distributedLock;
    }

    /// <summary>
    /// 尝试获取分布式锁，并把锁句柄包装为锁令牌
    /// </summary>
    /// <param name="resourceKey">资源键</param>
    /// <param name="expiry">过期时间</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>锁令牌，null 表示获取失败</returns>
    public async Task<ILockToken?> TryAcquireLockAsync(string resourceKey, TimeSpan expiry, CancellationToken cancellationToken = default)
    {
        var handle = await _distributedLock.TryAcquireAsync(resourceKey, expiry, cancellationToken);
        return handle is null ? null : new CachingJobLockToken(handle);
    }
}

/// <summary>
/// <see cref="ILockToken"/> 适配器：包装 <see cref="IResourceLockHandle"/>。
/// </summary>
internal sealed class CachingJobLockToken : ILockToken
{
    private readonly IResourceLockHandle _handle;

    public CachingJobLockToken(IResourceLockHandle handle)
    {
        _handle = handle;
    }

    public string ResourceKey => _handle.ResourceKey;

    public string LockId => _handle.LockId;

    public bool IsReleased => _handle.IsReleased;

    public Task ReleaseAsync()
    {
        return _handle.ReleaseAsync();
    }

    public void Dispose()
    {
        _handle.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _handle.DisposeAsync();
    }
}
