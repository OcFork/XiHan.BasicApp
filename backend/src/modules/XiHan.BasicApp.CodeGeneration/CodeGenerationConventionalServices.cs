// 由 scratchpad/gen_conventional_services.py 按 4.x 约定生成（仓储基类 / 应用服务基类 → 作用域 + 默认接口）；改动请改脚本重跑。

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace XiHan.BasicApp.CodeGeneration;

/// <summary>
/// CodeGeneration 模块按约定注册的仓储与应用服务（4.x 由框架基类上的依赖标记自动注册；5.0 改为显式登记）
/// </summary>
public static class CodeGenerationConventionalServices
{
    /// <summary>
    /// 注册派生自仓储基类与应用服务基类的全部具体类型：类本身 + 默认接口（类名以接口名去 I 结尾），作用域生命周期，幂等
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddCodeGenerationConventionalServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // 仓储
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Infrastructure.Repositories.CodeGenDataSourceRepository>();
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Domain.Repositories.ICodeGenDataSourceRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.CodeGeneration.Infrastructure.Repositories.CodeGenDataSourceRepository>());
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Infrastructure.Repositories.CodeGenHistoryRepository>();
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Domain.Repositories.ICodeGenHistoryRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.CodeGeneration.Infrastructure.Repositories.CodeGenHistoryRepository>());
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Infrastructure.Repositories.CodeGenTableColumnRepository>();
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Domain.Repositories.ICodeGenTableColumnRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.CodeGeneration.Infrastructure.Repositories.CodeGenTableColumnRepository>());
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Infrastructure.Repositories.CodeGenTableRepository>();
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Domain.Repositories.ICodeGenTableRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.CodeGeneration.Infrastructure.Repositories.CodeGenTableRepository>());
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Infrastructure.Repositories.CodeGenTemplateRepository>();
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Domain.Repositories.ICodeGenTemplateRepository>(static sp => sp.GetRequiredService<XiHan.BasicApp.CodeGeneration.Infrastructure.Repositories.CodeGenTemplateRepository>());

        // 应用与查询服务
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Application.AppServices.CodeGenDataSourceAppService>();
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Application.Contracts.ICodeGenDataSourceAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.CodeGeneration.Application.AppServices.CodeGenDataSourceAppService>());
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Application.AppServices.CodeGenTableAppService>();
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Application.Contracts.ICodeGenTableAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.CodeGeneration.Application.AppServices.CodeGenTableAppService>());
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Application.AppServices.CodeGenTableColumnAppService>();
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Application.Contracts.ICodeGenTableColumnAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.CodeGeneration.Application.AppServices.CodeGenTableColumnAppService>());
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Application.AppServices.CodeGenTemplateAppService>();
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Application.Contracts.ICodeGenTemplateAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.CodeGeneration.Application.AppServices.CodeGenTemplateAppService>());
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Application.AppServices.CodeGenerationAppService>();
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Application.Contracts.ICodeGenerationAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.CodeGeneration.Application.AppServices.CodeGenerationAppService>());
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Application.AppServices.DynamicRuntimeAppService>();
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Application.Contracts.IDynamicRuntimeAppService>(static sp => sp.GetRequiredService<XiHan.BasicApp.CodeGeneration.Application.AppServices.DynamicRuntimeAppService>());
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Application.QueryServices.CodeGenDataSourceQueryService>();
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Application.Contracts.ICodeGenDataSourceQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.CodeGeneration.Application.QueryServices.CodeGenDataSourceQueryService>());
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Application.QueryServices.CodeGenHistoryQueryService>();
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Application.Contracts.ICodeGenHistoryQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.CodeGeneration.Application.QueryServices.CodeGenHistoryQueryService>());
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Application.QueryServices.CodeGenTableColumnQueryService>();
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Application.Contracts.ICodeGenTableColumnQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.CodeGeneration.Application.QueryServices.CodeGenTableColumnQueryService>());
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Application.QueryServices.CodeGenTableQueryService>();
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Application.Contracts.ICodeGenTableQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.CodeGeneration.Application.QueryServices.CodeGenTableQueryService>());
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Application.QueryServices.CodeGenTemplateQueryService>();
        services.TryAddScoped<XiHan.BasicApp.CodeGeneration.Application.Contracts.ICodeGenTemplateQueryService>(static sp => sp.GetRequiredService<XiHan.BasicApp.CodeGeneration.Application.QueryServices.CodeGenTemplateQueryService>());

        return services;
    }
}
