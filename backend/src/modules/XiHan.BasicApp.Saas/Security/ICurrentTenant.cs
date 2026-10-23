// 由 xihan migrate v4 生成；提交前请检查。

namespace XiHan.BasicApp.Saas.Security;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public interface ICurrentTenant
{
    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    bool IsAvailable { get; }

    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    string? Id { get; }

    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    IDisposable Change(string? id, string? name = null);
}
