// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace XiHan.BasicApp.Core.Security;

/// <summary>
/// 当前租户的执行流环境（AsyncLocal）：调用携带的租户由管道中间件在进入时登记，显式切换（<see cref="ICurrentTenant.Change"/>）压在它之上。
/// 单例服务（SqlSugar 全局过滤器、后台任务）与作用域服务由此看到同一口径的"当前租户"。
/// </summary>
public static class TenantAmbient
{
    /// <summary>
    /// 调用携带的租户：中间件进入时设置、离开时恢复
    /// </summary>
    private static readonly AsyncLocal<Entry?> _invoked = new();

    /// <summary>
    /// 显式切换栈：Change 压入、释放弹出
    /// </summary>
    private static readonly AsyncLocal<Entry?> _override = new();

    /// <summary>
    /// 当前生效的租户标识；没有任何租户时为空（平台）
    /// </summary>
    public static string? Id => _override.Value is { } current ? current.Id : _invoked.Value?.Id;

    /// <summary>
    /// 当前生效的租户名称
    /// </summary>
    public static string? Name => _override.Value is { } current ? current.Name : _invoked.Value?.Name;

    /// <summary>
    /// 是否有显式切换在生效
    /// </summary>
    public static bool HasOverride => _override.Value is not null;

    /// <summary>
    /// 登记调用携带的租户；释放时恢复上一层
    /// </summary>
    /// <param name="id">租户标识；空白表示平台</param>
    /// <param name="name">租户名称</param>
    public static IDisposable EnterInvocation(string? id, string? name)
    {
        var previous = _invoked.Value;
        _invoked.Value = new Entry(Normalize(id), name, previous);
        return new Restore(_invoked, previous);
    }

    /// <summary>
    /// 显式切换到某租户（空白即平台）；释放时弹回上一层
    /// </summary>
    /// <param name="id">租户标识</param>
    /// <param name="name">租户名称</param>
    public static IDisposable Override(string? id, string? name)
    {
        var previous = _override.Value;
        _override.Value = new Entry(Normalize(id), name, previous);
        return new Restore(_override, previous);
    }

    /// <summary>
    /// 空白按空处理，其余去掉首尾空白
    /// </summary>
    private static string? Normalize(string? id) => string.IsNullOrWhiteSpace(id) ? null : id.Trim();

    /// <summary>
    /// 一层登记
    /// </summary>
    private sealed record Entry(string? Id, string? Name, Entry? Parent);

    /// <summary>
    /// 释放时恢复到登记前的一层
    /// </summary>
    private sealed class Restore(AsyncLocal<Entry?> slot, Entry? previous) : IDisposable
    {
        /// <summary>
        /// 是否已恢复
        /// </summary>
        private bool _disposed;

        /// <inheritdoc />
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            slot.Value = previous;
        }
    }
}
