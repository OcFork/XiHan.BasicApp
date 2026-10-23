// xihan-migrate(V4-03): 在特性的 Configure 里调用 AddAIV4Services()。
// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.Extensions.DependencyInjection;
using XiHan.BasicApp.AI.Application.AppServices;
using XiHan.BasicApp.AI.Application.QueryServices;

namespace XiHan.BasicApp.AI;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public static class AIV4Services
{
    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    public static IServiceCollection AddAIV4Services(this IServiceCollection services)
    {
        services.AddScoped<IAiAssistantAppService, AiAssistantAppService>();
        services.AddScoped<IAiPromptAppService, AiPromptAppService>();
        services.AddScoped<IAiProviderAppService, AiProviderAppService>();
        services.AddScoped<IChatAssistantAppService, ChatAssistantAppService>();
        services.AddScoped<IKnowledgeDocumentAppService, KnowledgeDocumentAppService>();
        services.AddScoped<IKnowledgeQueryAppService, KnowledgeQueryAppService>();
        services.AddScoped<IAiAssistantQueryService, AiAssistantQueryService>();
        services.AddScoped<IAiPromptQueryService, AiPromptQueryService>();
        services.AddScoped<IAiProviderQueryService, AiProviderQueryService>();
        services.AddScoped<IKnowledgeDocumentQueryService, KnowledgeDocumentQueryService>();
        return services;
    }
}
