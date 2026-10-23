// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using XiHan.BasicApp.Chat;
using XiHan.BasicApp.CodeGeneration;
using XiHan.BasicApp.Core;
using XiHan.BasicApp.Printing;
using XiHan.BasicApp.Saas;
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

// 5.0：能力包显式加入、不扫描。Full 组合包给出持久化 / 缓存 / 安全 / 消息 / 本地化 / 对象存储 / 调度 / 审计的默认实现与 HTTP / gRPC / MCP / CloudEvents 协议，
// SqlSugar Provider 顶替默认的内存持久化（客户端由 BasicApp.Core 自己注册），业务特性按依赖顺序加入；AI 与 Workflow 随 5.1 迁。
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
app.MapXiHanHealth();
app.MapXiHanOperations();
app.MapXiHanOpenApi();

await app.RunAsync();
