// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Logging;

namespace XiHan.BasicApp.Core.Exceptions;

/// <summary>
/// 可直接展示给用户的业务异常
/// </summary>
public class UserFriendlyException : BusinessException
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="code">错误码</param>
    /// <param name="details">详情</param>
    /// <param name="innerException">内部异常</param>
    /// <param name="logLevel">日志级别</param>
    public UserFriendlyException(string message, string? code = null, string? details = null, Exception? innerException = null, LogLevel logLevel = LogLevel.Warning)
        : base(code, message, details, innerException, logLevel)
    {
        Details = details;
    }

    /// <summary>
    /// 构造函数（可本地化消息）
    /// </summary>
    /// <param name="localizableMessage">可本地化的消息</param>
    /// <param name="fallbackMessage">回退消息</param>
    /// <param name="code">错误码</param>
    /// <param name="details">详情</param>
    /// <param name="innerException">内部异常</param>
    /// <param name="logLevel">日志级别</param>
    public UserFriendlyException(object localizableMessage, string? fallbackMessage = null, string? code = null, string? details = null, Exception? innerException = null, LogLevel logLevel = LogLevel.Warning)
        : base(code, fallbackMessage, details, innerException, logLevel)
    {
        Details = details;
        LocalizableMessage = localizableMessage;
    }
}
