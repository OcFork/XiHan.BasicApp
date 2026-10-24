// xihan-migrate(V4-03): 在特性的 Configure 里调用 AddCodeGenerationV4Services()。
// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.Extensions.DependencyInjection;
using XiHan.BasicApp.CodeGeneration.Application.AppServices;
using XiHan.BasicApp.CodeGeneration.Application.Contracts;
using XiHan.BasicApp.CodeGeneration.Application.QueryServices;

namespace XiHan.BasicApp.CodeGeneration;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public static class CodeGenerationV4Services
{
    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    public static IServiceCollection AddCodeGenerationV4Services(this IServiceCollection services)
    {
        services.AddScoped<ICodeGenDataSourceAppService, CodeGenDataSourceAppService>();
        services.AddScoped<ICodeGenTableAppService, CodeGenTableAppService>();
        services.AddScoped<ICodeGenTableColumnAppService, CodeGenTableColumnAppService>();
        services.AddScoped<ICodeGenTemplateAppService, CodeGenTemplateAppService>();
        services.AddScoped<ICodeGenerationAppService, CodeGenerationAppService>();
        services.AddScoped<IDynamicRuntimeAppService, DynamicRuntimeAppService>();
        services.AddScoped<ICodeGenDataSourceQueryService, CodeGenDataSourceQueryService>();
        services.AddScoped<ICodeGenHistoryQueryService, CodeGenHistoryQueryService>();
        services.AddScoped<ICodeGenTableColumnQueryService, CodeGenTableColumnQueryService>();
        services.AddScoped<ICodeGenTableQueryService, CodeGenTableQueryService>();
        services.AddScoped<ICodeGenTemplateQueryService, CodeGenTemplateQueryService>();
        return services;
    }
}
