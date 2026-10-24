// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Chat.Hubs;
using XiHan.BasicApp.Chat;
using XiHan.BasicApp.CodeGeneration;
using XiHan.BasicApp.Core.Bot.Telegram.Webhook;
using XiHan.BasicApp.Core;
using XiHan.BasicApp.Printing;
using XiHan.BasicApp.Saas.Hubs;
using XiHan.BasicApp.Saas.Infrastructure.OAuth;
using XiHan.BasicApp.Saas;
using XiHan.BasicApp.Web.Core.RealTime.Constants;
using XiHan.BasicApp.Web.Core.RealTime.Extensions;
using XiHan.BasicApp.Web.Core;
using XiHan.BasicApp.WebHost;
using XiHan.Framework.Hosting.AspNetCore;
using XiHan.Framework.Kernel.Hosting;
using XiHan.Framework.Persistence.SqlSugar;
using XiHan.Framework.Profiles.Full;
using XiHan.Framework.Protocols.Http;

var builder = WebApplication.CreateBuilder(args);

var configuredUrls = builder.Configuration["Hosting:Urls"];
if (!string.IsNullOrWhiteSpace(configuredUrls))
{
    builder.WebHost.UseUrls(configuredUrls);
}

// 5.0 宿主：Full 画像 + SqlSugar 持久化 + 应用特性（Core → Web.Core → 模块 → 宿主）
builder.AddXiHan(xihan => xihan
    .UseFullProfile()
    .UseFeature(new SqlSugarPersistenceFeature())
    .UseFeature<BasicAppCoreFeature>()
    .UseFeature<BasicAppWebCoreFeature>()
    .UseFeature<BasicAppSaasFeature>()
    .UseFeature<BasicAppCodeGenerationFeature>()
    .UseFeature<BasicAppPrintingFeature>()
    .UseFeature<BasicAppChatFeature>()
    .UseFeature<BasicAppWebHostFeature>());

var app = builder.Build();

app.UseXiHan();

// Telegram Webhook 回调（原 4.x 宿主模块里的中间件）
app.UseTelegramBotWebhook();

// 5.0 探针与操作端点
app.MapXiHanHealth();
app.MapXiHanOperations();
app.MapXiHanOpenApi();

// 应用自身的端点（原各模块 StartAsync 里经 GetApplicationBuilder 注册的部分）
app.MapBasicAppHealth();
app.MapXiHanHub<BasicAppNotificationHub>(SignalRConstants.HubPaths.Notification);
app.MapXiHanHub<BasicAppChatHub>(SignalRConstants.HubPaths.Chat);
app.MapOAuthEndpoints();
app.MapOAuthConnectEndpoints();
app.MapOidcEndpoints();

await app.RunAsync();
