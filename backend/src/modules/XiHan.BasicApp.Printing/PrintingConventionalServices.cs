// 由 scratchpad/gen_conventional_services.py 按 4.x 约定生成（仓储基类 / 应用服务基类 → 作用域 + 默认接口）；改动请改脚本重跑。

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace XiHan.BasicApp.Printing;

/// <summary>
/// Printing 模块按约定注册的仓储与应用服务（4.x 由框架基类上的依赖标记自动注册；5.0 改为显式登记）
/// </summary>
public static class PrintingConventionalServices
{
    /// <summary>
    /// 注册派生自仓储基类与应用服务基类的全部具体类型：类本身 + 默认接口（类名以接口名去 I 结尾），作用域生命周期，幂等
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddPrintingConventionalServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // 仓储
        services.TryAddScoped<XiHan.BasicApp.Printing.Infrastructure.Repositories.PrintTemplateRepository>();
        services.TryAddScoped<XiHan.BasicApp.Printing.Domain.Repositories.IPrintTemplateRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.Printing.Infrastructure.Repositories.PrintTemplateRepository>());

        // 应用与查询服务
        services.TryAddScoped<XiHan.BasicApp.Printing.Application.AppServices.PrintTemplateAppService>();
        services.TryAddScoped<XiHan.BasicApp.Printing.Application.Contracts.IPrintTemplateAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Printing.Application.AppServices.PrintTemplateAppService>());
        services.TryAddScoped<XiHan.BasicApp.Printing.Application.QueryServices.PrintDataSourceQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Printing.Application.Contracts.IPrintDataSourceQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Printing.Application.QueryServices.PrintDataSourceQueryService>());
        services.TryAddScoped<XiHan.BasicApp.Printing.Application.QueryServices.PrintTemplateQueryService>();
        services.TryAddScoped<XiHan.BasicApp.Printing.Application.Contracts.IPrintTemplateQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.Printing.Application.QueryServices.PrintTemplateQueryService>());

        return services;
    }
}
