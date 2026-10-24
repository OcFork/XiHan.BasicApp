// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace XiHan.BasicApp.Saas.Infrastructure.Caching;

/// <summary>
/// 默认延迟队列（有界进程内实现；承接 4.x 框架 Caching 模块的同名实现）
/// </summary>
/// <remarks>
/// 消息仅驻留当前进程内存，<b>不跨实例、进程退出即丢失</b>；多实例部署或需要消息持久化时启用 Redis 改用 <see cref="RedisDelayQueue{T}"/>。
/// 优先级为 <c>(到期时间戳ms, 入队序号)</c>，同一到期时刻按入队先后取出。注册为单例（每个封闭类型一个实例）。
/// </remarks>
/// <typeparam name="T">消息类型</typeparam>
public sealed class DefaultDelayQueue<T> : IRedisDelayQueue<T>
{
    /// <summary>
    /// 容量上限：进程内队列不能无界增长
    /// </summary>
    private const int MaxItemCount = 100000;

    private readonly Lock _gate = new();
    private readonly PriorityQueue<T, (long DueAtMs, long Sequence)> _queue = new();

    private long _sequence;

    /// <inheritdoc />
    public Task EnqueueAsync(T item, TimeSpan delay, CancellationToken cancellationToken = default)
    {
        return EnqueueAtAsync(item, DateTimeOffset.UtcNow.Add(delay), cancellationToken);
    }

    /// <inheritdoc />
    public Task EnqueueAtAsync(T item, DateTimeOffset dueTime, CancellationToken cancellationToken = default)
    {
        lock (_gate)
        {
            // 到上限直接拒绝而不是静默丢弃：让调用方知道该换持久化实现
            if (_queue.Count >= MaxItemCount)
            {
                throw new InvalidOperationException($"The default delay queue reached its limit of {MaxItemCount} items; use the Redis implementation instead.");
            }

            _queue.Enqueue(item, (dueTime.ToUnixTimeMilliseconds(), _sequence++));
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<T>> DequeueDueAsync(int count, CancellationToken cancellationToken = default)
    {
        if (count <= 0)
        {
            return Task.FromResult<IReadOnlyList<T>>([]);
        }

        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var due = new List<T>();

        lock (_gate)
        {
            while (due.Count < count && _queue.TryPeek(out _, out var priority) && priority.DueAtMs <= now)
            {
                due.Add(_queue.Dequeue());
            }
        }

        return Task.FromResult<IReadOnlyList<T>>(due);
    }

    /// <inheritdoc />
    public Task<long> CountAsync(CancellationToken cancellationToken = default)
    {
        lock (_gate)
        {
            return Task.FromResult((long)_queue.Count);
        }
    }
}
