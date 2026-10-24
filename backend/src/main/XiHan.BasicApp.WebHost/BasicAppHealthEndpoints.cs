// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace XiHan.BasicApp.WebHost;

/// <summary>
/// 应用自身的 ASP.NET 健康检查端点（数据库、Redis 等检查项）；5.0 的探针在 <c>/health/startup|ready|live</c>
/// </summary>
public static class BasicAppHealthEndpoints
{
    /// <summary>
    /// 映射 <c>/health</c>：匿名可访问，只返回总状态与各检查项名称、状态
    /// </summary>
    /// <param name="endpoints">端点路由</param>
    /// <param name="pattern">路径</param>
    public static IEndpointConventionBuilder MapBasicAppHealth(this IEndpointRouteBuilder endpoints, string pattern = "/health")
    {
        ArgumentNullException.ThrowIfNull(endpoints);
        var options = new HealthCheckOptions
        {
            ResponseWriter = WriteMinimalHealthResponseAsync,
        };
        return endpoints.MapHealthChecks(pattern, options).AllowAnonymous();
    }

    /// <summary>
    /// 最小化健康响应：仅总状态 + 各检查项名称与状态，不含连接串/异常/描述等敏感细节
    /// </summary>
    private static Task WriteMinimalHealthResponseAsync(HttpContext httpContext, HealthReport report)
    {
        httpContext.Response.ContentType = "application/json; charset=utf-8";
        var payload = new
        {
            status = report.Status.ToString(),
            totalDurationMs = report.TotalDuration.TotalMilliseconds,
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
            }),
        };
        return httpContext.Response.WriteAsJsonAsync(payload);
    }
}
