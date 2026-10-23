// xihan-migrate(V4-03): 在特性的 Configure 里调用 AddChatV4Services()。
// 由 xihan migrate v4 生成；提交前请检查。

using Microsoft.Extensions.DependencyInjection;
using XiHan.BasicApp.Chat.Application.AppServices;
using XiHan.BasicApp.Chat.Application.EventHandlers;
using XiHan.BasicApp.Chat.Application.QueryServices;
using XiHan.BasicApp.Chat.Application.Services;
using XiHan.Framework.Messaging.Abstractions;

namespace XiHan.BasicApp.Chat;

/// <summary>
/// 由 xihan migrate v4 生成；提交前请检查。
/// </summary>
public static class ChatV4Services
{
    /// <summary>
    /// 由 xihan migrate v4 生成；提交前请检查。
    /// </summary>
    public static IServiceCollection AddChatV4Services(this IServiceCollection services)
    {
        services.AddScoped<IChatAppService, ChatAppService>();
        services.AddMessageHandler<ChatDepartmentMemberSyncEventHandler>(ChatDepartmentMemberSyncEventHandler.UserDepartmentChangedTopic);
        services.AddScoped<IChatAuditQueryService, ChatAuditQueryService>();
        services.AddScoped<IChatQueryService, ChatQueryService>();
        services.AddScoped<IChatRealtimePushService, ChatRealtimePushService>();
        services.AddScoped<IChatSensitiveWordGuard, ChatSensitiveWordGuard>();
        return services;
    }
}
