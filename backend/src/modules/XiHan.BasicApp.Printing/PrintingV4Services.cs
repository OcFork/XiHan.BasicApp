// xihan-migrate(V4-03): 在特性的 Configure 里调用 AddPrintingV4Services()。
// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.Extensions.DependencyInjection;
using XiHan.BasicApp.Printing.Application.AppServices;
using XiHan.BasicApp.Printing.Application.Contracts;
using XiHan.BasicApp.Printing.Application.QueryServices;

namespace XiHan.BasicApp.Printing;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public static class PrintingV4Services
{
    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    public static IServiceCollection AddPrintingV4Services(this IServiceCollection services)
    {
        services.AddScoped<IPrintTemplateAppService, PrintTemplateAppService>();
        services.AddScoped<IPrintDataSourceQueryService, PrintDataSourceQueryService>();
        services.AddScoped<IPrintTemplateQueryService, PrintTemplateQueryService>();
        return services;
    }
}
