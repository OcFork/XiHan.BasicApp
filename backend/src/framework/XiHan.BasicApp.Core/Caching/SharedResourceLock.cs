// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.Framework.Caching.Abstractions.Locking;
using XiHan.Framework.Caching.Abstractions.Stores;
using XiHan.Framework.Caching.Abstractions;

namespace XiHan.BasicApp.Core.Caching;

/// <summary>
/// 把 4.x 形态的 <see cref="IResourceLock"/> 架在 5.0 的 <see cref="IDistributedLock"/> 上
/// </summary>
/// <param name="distributedLock">5.0 分布式锁</param>
public sealed class SharedResourceLock(IDistributedLock distributedLock) : IResourceLock
{
    /// <inheritdoc />
    public async Task<IResourceLockHandle?> TryAcquireAsync(string resourceKey, TimeSpan expiry, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(resourceKey);
        var handle = await distributedLock.TryAcquireAsync(new CacheKey(resourceKey), expiry, cancellationToken).ConfigureAwait(false);
        return handle.TryGetValue(out var acquired) ? new Handle(resourceKey, acquired) : null;
    }

    /// <summary>
    /// 锁句柄：释放委托给 5.0 句柄；续期暂不支持
    /// </summary>
    private sealed class Handle(string resourceKey, LockHandle handle) : IResourceLockHandle
    {
        /// <inheritdoc />
        public string ResourceKey => resourceKey;

        /// <inheritdoc />
        public string LockId => handle.Token;

        /// <inheritdoc />
        public bool IsReleased { get; private set; }

        /// <inheritdoc />
        public async Task ReleaseAsync()
        {
            if (IsReleased)
            {
                return;
            }

            IsReleased = true;
            await handle.ReleaseAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <inheritdoc />
        // xihan-migrate(P5-09): 框架缺口——5.0 LockHandle 没有续期；补上前续期一律返回 false，持锁方按"已失去锁"提前结束。
        public Task<bool> ExtendAsync(TimeSpan expiry, CancellationToken cancellationToken = default) => Task.FromResult(false);

        /// <inheritdoc />
        public ValueTask DisposeAsync() => new(ReleaseAsync());

        /// <inheritdoc />
        public void Dispose()
        {
            // 同步释放只为兼容 4.x 的 using 用法；优先走 DisposeAsync
            ReleaseAsync().GetAwaiter().GetResult();
        }
    }
}
