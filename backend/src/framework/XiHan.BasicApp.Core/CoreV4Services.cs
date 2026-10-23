// xihan-migrate(V4-03): 在特性的 Configure 里调用 AddCoreV4Services()。
// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.Extensions.DependencyInjection;
using XiHan.BasicApp.Core.Data.SqlSugar.Initializers;

namespace XiHan.BasicApp.Core;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public static class CoreV4Services
{
    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    public static IServiceCollection AddCoreV4Services(this IServiceCollection services)
    {
        services.AddScoped<IDbInitializer, DbInitializer>();
        return services;
    }
}
