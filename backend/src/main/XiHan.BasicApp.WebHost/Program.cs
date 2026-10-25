// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Chat.Hubs;
using XiHan.BasicApp.Chat;
using XiHan.BasicApp.CodeGeneration;
using XiHan.BasicApp.Core.Bot.Telegram.Webhook;
using XiHan.BasicApp.Core;
using XiHan.BasicApp.Printing;
using XiHan.BasicApp.Saas.Hubs;
using XiHan.BasicApp.Saas.Infrastructure.Files;
using XiHan.BasicApp.Saas.Infrastructure.OAuth;
using XiHan.BasicApp.Saas;
using XiHan.BasicApp.Web.Core.RealTime.Constants;
using XiHan.BasicApp.Web.Core.RealTime.Extensions;
using XiHan.BasicApp.Web.Core;
using XiHan.BasicApp.WebHost;
using XiHan.Framework.Caching.Redis;
using XiHan.Framework.Hosting.AspNetCore;
using XiHan.Framework.Kernel.Hosting;
using XiHan.Framework.Persistence.SqlSugar;
using XiHan.Framework.Profiles.Full;
using XiHan.Framework.Protocols.Http;
using XiHan.Framework.Security.Jwt;

var builder = WebApplication.CreateBuilder(args);

var configuredUrls = builder.Configuration["Hosting:Urls"];
if (!string.IsNullOrWhiteSpace(configuredUrls))
{
    builder.WebHost.UseUrls(configuredUrls);
}

// 5.0 宿主：Full 画像 + SqlSugar 持久化 + 应用特性（Core → Web.Core → 模块 → 宿主）
// 5.0 的 JWT 认证与多租户解析复用 4.x 的配置：签发（XiHan:Authentication:Jwt）与校验（XiHan:Security:Jwt）同一把密钥；
// 租户来自令牌的 tenantid 声明，租户目录在应用自己的库里，不校验画像里的内存租户表
var jwt = builder.Configuration.GetSection("XiHan:Authentication:Jwt");
builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
{
    ["XiHan:Security:Jwt:Secret"] = builder.Configuration["XiHan:Security:Jwt:Secret"] ?? jwt["SecretKey"],
    ["XiHan:Security:Jwt:Issuer"] = builder.Configuration["XiHan:Security:Jwt:Issuer"] ?? jwt["Issuer"],
    ["XiHan:Security:Jwt:Audience"] = builder.Configuration["XiHan:Security:Jwt:Audience"] ?? jwt["Audience"],
    ["XiHan:Security:Jwt:ClockSkewSeconds"] = builder.Configuration["XiHan:Security:Jwt:ClockSkewSeconds"] ?? (jwt.GetValue<int?>("ClockSkewMinutes") is { } skew ? (skew * 60).ToString() : null),
    ["XiHan:MultiTenancy:TenantClaim"] = builder.Configuration["XiHan:MultiTenancy:TenantClaim"] ?? "tenantid",
    ["XiHan:MultiTenancy:ValidateAgainstStore"] = builder.Configuration["XiHan:MultiTenancy:ValidateAgainstStore"] ?? "false",
});

// Redis 缓存 Provider：顶替画像里的内存缓存（Saas 的登录封禁、延迟队列、按前缀失效都依赖 Redis）
var redisEnabled = builder.Configuration.GetValue<bool>("XiHan:Caching:Redis:IsEnabled");

builder.AddXiHan(xihan =>
{
    xihan.UseFullProfile();
    xihan.UseFeature<JwtSecurityFeature>();
    if (redisEnabled)
    {
        xihan.UseFeature<RedisCachingFeature>();
    }

    xihan.UseFeature(new SqlSugarPersistenceFeature())
        .UseFeature<BasicAppCoreFeature>()
        .UseFeature<BasicAppWebCoreFeature>()
        .UseFeature<BasicAppSaasFeature>()
        .UseFeature<BasicAppCodeGenerationFeature>()
        .UseFeature<BasicAppPrintingFeature>()
        .UseFeature<BasicAppChatFeature>()
        .UseFeature<BasicAppWebHostFeature>();
});

var app = builder.Build();

app.UseXiHan();

// ASP.NET Core 自身的认证与授权：只对带 [Authorize] / RequireAuthorization 的端点生效（SignalR Hub、OIDC UserInfo、第三方登录），5.0 操作端点由内核管道授权
app.UseAuthentication();
app.UseAuthorization();

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
// 文件上传（multipart）与下载（流）走 ASP.NET 端点，路径沿用 5.0 集合级动词形态 /api/file:upload、/api/file:download
app.MapFileTransferEndpoints();
app.MapOAuthEndpoints();
app.MapOAuthConnectEndpoints();
app.MapOidcEndpoints();

await app.RunAsync();
