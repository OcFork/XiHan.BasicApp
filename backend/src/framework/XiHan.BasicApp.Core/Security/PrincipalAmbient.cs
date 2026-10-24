// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Security.Claims;

namespace XiHan.BasicApp.Core.Security;

/// <summary>
/// 执行流上的主体覆盖：后台任务（导出、调度）要以某个用户的身份执行一段代码时在这里登记，
/// <see cref="ICurrentUser"/> 与 <see cref="ICurrentPrincipalAccessor"/> 优先读它，没有覆盖时才读调用携带的 5.0 主体
/// </summary>
public static class PrincipalAmbient
{
    private static readonly AsyncLocal<ClaimsPrincipal?> _override = new();

    /// <summary>
    /// 当前覆盖的主体；没有覆盖时为 null
    /// </summary>
    public static ClaimsPrincipal? Current => _override.Value;

    /// <summary>
    /// 覆盖当前主体，释放返回值时恢复外层
    /// </summary>
    /// <param name="principal">要生效的主体</param>
    /// <returns>恢复句柄</returns>
    public static IDisposable Override(ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);
        var previous = _override.Value;
        _override.Value = principal;
        return new Restore(previous);
    }

    /// <summary>
    /// 恢复外层主体
    /// </summary>
    private sealed class Restore(ClaimsPrincipal? previous) : IDisposable
    {
        private bool _disposed;

        /// <inheritdoc />
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _override.Value = previous;
        }
    }
}
