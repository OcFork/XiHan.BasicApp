// 由 xihan migrate v4 生成；提交前请检查。

using XiHan.Framework.Kernel.Pipeline;
using XiHan.Framework.Kernel.Security;

namespace XiHan.BasicApp.Core.Security;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public sealed class CurrentTenant(IInvocationAccessor accessor) : ICurrentTenant
{
    private TenantContext? Tenant => accessor.Current.TryGetValue(out var context) ? context.GetSecurity().Tenant : null;

    /// <inheritdoc />
    public bool IsAvailable => Tenant is not null;

    /// <inheritdoc />
    public string? Id => Tenant?.Id;

    /// <inheritdoc />
    public string? Name => Tenant?.DisplayName;

    /// <inheritdoc />
    public IDisposable Change(string? id, string? name = null) => throw new NotSupportedException("The tenant follows the invocation's baggage and cannot be changed inside a call; start a new invocation with the tenant baggage instead.");
}
