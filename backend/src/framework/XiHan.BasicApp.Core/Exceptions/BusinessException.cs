// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Logging;

namespace XiHan.BasicApp.Core.Exceptions;

/// <summary>
/// 业务异常：带错误码、详情与日志级别，由应用层的异常映射中间件翻译成失败结果
/// </summary>
/// <remarks>5.0 框架内部只用 <c>Result</c> / <c>Error</c> 表达失败；应用层沿用异常风格时在 Egress 阶段统一映射。</remarks>
public class BusinessException : Exception
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="code">错误码</param>
    /// <param name="message">消息</param>
    /// <param name="details">详情</param>
    /// <param name="innerException">内部异常</param>
    /// <param name="logLevel">日志级别</param>
    public BusinessException(string? code = null, string? message = null, string? details = null, Exception? innerException = null, LogLevel logLevel = LogLevel.Warning)
        : base(message, innerException)
    {
        Code = code;
        Details = details;
        LogLevel = logLevel;
    }

    /// <summary>
    /// 错误码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 可本地化的消息（消息键或模板对象）
    /// </summary>
    public object? LocalizableMessage { get; set; }

    /// <summary>
    /// 详情
    /// </summary>
    public string? Details { get; set; }

    /// <summary>
    /// 日志级别
    /// </summary>
    public LogLevel LogLevel { get; set; }

    /// <summary>
    /// 附加数据
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="value">值</param>
    /// <returns>当前异常</returns>
    public BusinessException WithData(string name, object value)
    {
        Data[name] = value;
        return this;
    }
}
