// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace XiHan.BasicApp.Core.Bot.Helpers;

/// <summary>
/// 机器人 Webhook 的 HTTP 调用（替代 4.x 的 <c>url.AsHttp()</c> 流式 API）：共享一个客户端，响应按 JSON 反序列化
/// </summary>
public static class WebhookHttp
{
    /// <summary>
    /// 共享客户端：连接池按生命周期轮换，避免长期进程里的 DNS 变更失效
    /// </summary>
    private static readonly HttpClient _client = new(new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan.FromMinutes(5) })
    {
        Timeout = TimeSpan.FromSeconds(30),
    };

    /// <summary>
    /// JSON 选项：Web 约定（驼峰、忽略大小写）
    /// </summary>
    private static readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web);

    /// <summary>
    /// POST JSON 请求体
    /// </summary>
    /// <typeparam name="T">响应类型</typeparam>
    /// <param name="url">地址</param>
    /// <param name="body">请求体对象</param>
    /// <param name="cancellationToken">取消令牌</param>
    public static async Task<WebhookResult<T>> PostJsonAsync<T>(string url, object body, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(url);
        using var content = new StringContent(JsonSerializer.Serialize(body, body.GetType(), _json), Encoding.UTF8, "application/json");
        return await SendAsync<T>(new HttpRequestMessage(HttpMethod.Post, url) { Content = content }, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// POST 原始流（如 multipart 上传），可附带请求头
    /// </summary>
    /// <typeparam name="T">响应类型</typeparam>
    /// <param name="url">地址</param>
    /// <param name="stream">请求体流</param>
    /// <param name="headers">请求头；<c>Content-Type</c> 落到内容头，其余落到请求头</param>
    /// <param name="cancellationToken">取消令牌</param>
    public static async Task<WebhookResult<T>> PostStreamAsync<T>(string url, Stream stream, IEnumerable<KeyValuePair<string, string>>? headers, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(url);
        ArgumentNullException.ThrowIfNull(stream);
        using var content = new StreamContent(stream);
        using var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = content };
        foreach (var (name, value) in headers ?? [])
        {
            if (name.Equals("Content-Type", StringComparison.OrdinalIgnoreCase))
            {
                content.Headers.ContentType = MediaTypeHeaderValue.Parse(value);
            }
            else if (name.StartsWith("Content-", StringComparison.OrdinalIgnoreCase))
            {
                content.Headers.TryAddWithoutValidation(name, value);
            }
            else
            {
                request.Headers.TryAddWithoutValidation(name, value);
            }
        }

        return await SendAsync<T>(request, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// 发送并读取响应：状态码成功且正文可按 JSON 解析才算成功；原文始终保留给调用方做错误回显
    /// </summary>
    private static async Task<WebhookResult<T>> SendAsync<T>(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        using var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        var raw = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode || string.IsNullOrWhiteSpace(raw))
        {
            return new WebhookResult<T>(false, default, raw);
        }

        try
        {
            return new WebhookResult<T>(true, JsonSerializer.Deserialize<T>(raw, _json), raw);
        }
        catch (JsonException)
        {
            return new WebhookResult<T>(false, default, raw);
        }
    }
}

/// <summary>
/// Webhook 调用结果
/// </summary>
/// <typeparam name="T">响应类型</typeparam>
/// <param name="IsSuccess">HTTP 成功且响应可解析</param>
/// <param name="Data">解析出的响应</param>
/// <param name="RawDataString">响应原文</param>
public sealed record WebhookResult<T>(bool IsSuccess, T? Data, string RawDataString);
