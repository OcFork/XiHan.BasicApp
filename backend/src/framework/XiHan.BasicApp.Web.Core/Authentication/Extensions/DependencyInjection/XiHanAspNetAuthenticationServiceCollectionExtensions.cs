// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using XiHan.BasicApp.Web.Core.Authentication.Jwt;
using XiHan.BasicApp.Web.Core.Authentication.OAuth;

namespace XiHan.BasicApp.Web.Core.Authentication.Extensions.DependencyInjection;

/// <summary>
/// ASP.NET Core 自身的认证（承接 4.x Web.Api 模块的 <c>AddXiHanWebApiAuth</c>）：
/// 5.0 的操作由内核管道认证授权，但 SignalR Hub 的 <c>[Authorize]</c>、OIDC UserInfo 的 <c>RequireAuthorization()</c>
/// 与第三方登录的 Challenge / 回调仍走 ASP.NET Core 的认证中间件，这里用同一把 JWT 密钥配置 Bearer 方案，
/// 并为第三方登录注册临时 Cookie 方案；不设回退策略，免得把 5.0 操作端点也拦下
/// </summary>
public static class XiHanAspNetAuthenticationServiceCollectionExtensions
{
    /// <summary>
    /// 4.x Web 认证配置节（只用其中的 <c>SignalRHubPathPrefix</c>）
    /// </summary>
    public const string WebAuthSectionName = "XiHan:Web:Api:Auth";

    /// <summary>
    /// 没有配置时的 Hub 路径前缀：WebSocket / SSE 不能带 Authorization 头，这些路径从查询串 <c>access_token</c> 取令牌
    /// </summary>
    public const string DefaultSignalRHubPathPrefix = "/hubs";

    /// <summary>
    /// 注册 ASP.NET Core 认证与授权：JWT Bearer 为默认认证 / 挑战方案（密钥、签发者、受众、时钟偏移来自 <c>XiHan:Authentication:Jwt</c>），
    /// 第三方登录启用时追加 <see cref="XiHanOAuthServiceCollectionExtensions.ExternalSignInScheme"/> Cookie 方案；必须先于 <c>AddXiHanAuthentication</c> 调用，
    /// 后者的 OAuth Provider 会复用这里建立的认证构建器
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddXiHanAspNetAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var jwt = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();
        var hasJwt = jwt is not null && !string.IsNullOrWhiteSpace(jwt.SecretKey);
        var hubPrefix = configuration[$"{WebAuthSectionName}:SignalRHubPathPrefix"] is { Length: > 0 } prefix ? prefix : DefaultSignalRHubPathPrefix;

        // 没有 JWT 配置时仍注册认证服务，让 Challenge 以明确的「无默认方案」失败而不是找不到服务
        var builder = hasJwt
            ? services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            : services.AddAuthentication();

        if (hasJwt)
        {
            builder.AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = jwt!.ValidateIssuer,
                    ValidateAudience = jwt.ValidateAudience,
                    ValidateLifetime = jwt.ValidateLifetime,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey)),
                    ClockSkew = TimeSpan.FromMinutes(jwt.ClockSkewMinutes),
                };
                options.Events = new JwtBearerEvents
                {
                    // SignalR 的 WebSocket / SSE 连接带不了 Authorization 头，只能从查询串取
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        if (!string.IsNullOrEmpty(accessToken) && context.HttpContext.Request.Path.StartsWithSegments(hubPrefix))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    },
                    // 过期单独打标，前端据此刷新令牌而不是直接登出
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception is SecurityTokenExpiredException)
                        {
                            context.Response.Headers["Token-Expired"] = "true";
                        }

                        return Task.CompletedTask;
                    },
                };
            });
        }

        // 第三方登录在跳转期间用临时 Cookie 保存外部身份
        var oauth = configuration.GetSection(OAuthOptions.SectionName).Get<OAuthOptions>();
        if (oauth is { Enabled: true })
        {
            builder.AddCookie(XiHanOAuthServiceCollectionExtensions.ExternalSignInScheme, cookie =>
            {
                cookie.Cookie.Name = ".XiHan.External";
                cookie.Cookie.HttpOnly = true;
                cookie.Cookie.SameSite = SameSiteMode.Lax;
                cookie.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            });
        }

        // 只给带 [Authorize] / RequireAuthorization 的 ASP.NET 端点用；5.0 操作端点由内核管道授权，不设回退策略
        services.AddAuthorization();
        return services;
    }
}
