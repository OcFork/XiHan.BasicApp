// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.Framework.Kernel.Pipeline;
using XiHan.Framework.Kernel.Security;

namespace XiHan.BasicApp.Core.Security;

/// <summary>
/// 当前租户：默认跟随调用携带的租户；<see cref="Change"/> 在当前执行流里显式切换（平台维护、种子、后台任务按租户切入）
/// </summary>
public sealed class CurrentTenant(IInvocationAccessor accessor) : ICurrentTenant
{
    /// <summary>
    /// 显式切换栈：Change 压入、释放弹出，随执行流（AsyncLocal）传播，覆盖调用携带的租户
    /// </summary>
    private static readonly AsyncLocal<Override?> _override = new();

    /// <summary>
    /// 调用携带的租户
    /// </summary>
    private TenantContext? Invoked => accessor.Current.TryGetValue(out var context) ? context.GetSecurity().Tenant : null;

    /// <inheritdoc />
    public bool IsAvailable => Id is not null;

    /// <inheritdoc />
    public string? Id => _override.Value is { } current ? current.Id : Invoked?.Id;

    /// <inheritdoc />
    public string? Name => _override.Value is { } current ? current.Name : Invoked?.DisplayName;

    /// <inheritdoc />
    public IDisposable Change(string? id, string? name = null)
    {
        var parent = _override.Value;
        _override.Value = new Override(string.IsNullOrWhiteSpace(id) ? null : id.Trim(), name, parent);
        return new Restore(parent);
    }

    /// <summary>
    /// 一层切换；<c>Id</c> 为空表示切到平台
    /// </summary>
    private sealed record Override(string? Id, string? Name, Override? Parent);

    /// <summary>
    /// 释放时恢复到切换前的一层
    /// </summary>
    private sealed class Restore(Override? parent) : IDisposable
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
            _override.Value = parent;
        }
    }
}
