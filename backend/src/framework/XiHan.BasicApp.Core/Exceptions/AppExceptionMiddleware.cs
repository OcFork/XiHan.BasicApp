// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Logging;
using XiHan.Framework.Kernel;
using XiHan.Framework.Kernel.Pipeline;
using XiHan.Framework.Primitives;

namespace XiHan.BasicApp.Core.Exceptions;

/// <summary>
/// 把应用抛出的业务异常翻译成调用结果（承接 4.x <c>XiHanApiResponseResultFilter.MapException</c> 的状态语义）：
/// 业务 / 用户友好 / 参数 / 业务规则异常 → VALIDATION（400），并发冲突 → CONFLICT（409），键不存在 → NOTFOUND（404），
/// 未授权访问 → DENIED（403）；其余异常原样抛出，由内核 Observe 阶段记为 UNEXPECTED（500）
/// </summary>
/// <remarks>
/// 挂在 Observe 阶段的末尾、内核 ObserveMiddleware 之内：翻译后的结果仍由内核记指标，但不再按未预期异常打错误日志。
/// 消息是面向用户的中文文案，经 <see cref="Error.Literal"/> 原样携带（异常转换是 Literal 的允许场景）。
/// </remarks>
/// <param name="logger">日志</param>
public sealed class AppExceptionMiddleware(ILogger<AppExceptionMiddleware> logger) : IPipelineMiddleware
{
    /// <summary>
    /// 业务异常（含用户友好异常）
    /// </summary>
    public const string BusinessCode = "BASICAPP_VALIDATION_001";

    /// <summary>
    /// 领域规则被破坏（<see cref="BusinessRuleValidationException"/> 及其他 <see cref="DomainException"/>）
    /// </summary>
    public const string DomainRuleCode = "BASICAPP_VALIDATION_002";

    /// <summary>
    /// 参数非法（<see cref="ArgumentException"/>）
    /// </summary>
    public const string ArgumentCode = "BASICAPP_VALIDATION_003";

    /// <summary>
    /// 当前状态不允许该操作（<see cref="InvalidOperationException"/>：验证码失效、密码不合规等；4.x 为 422）
    /// </summary>
    public const string InvalidOperationCode = "BASICAPP_VALIDATION_004";

    /// <summary>
    /// 乐观并发冲突
    /// </summary>
    public const string ConcurrencyCode = "BASICAPP_CONFLICT_001";

    /// <summary>
    /// 目标不存在（<see cref="KeyNotFoundException"/>）
    /// </summary>
    public const string NotFoundCode = "BASICAPP_NOTFOUND_001";

    /// <summary>
    /// 未授权访问（<see cref="UnauthorizedAccessException"/>）
    /// </summary>
    public const string UnauthorizedCode = "BASICAPP_DENIED_001";

    /// <inheritdoc />
    public async ValueTask InvokeAsync(InvocationContext context, PipelineStep nextStep)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(nextStep);
        try
        {
            await nextStep(context).ConfigureAwait(false);
        }
        catch (Exception exception) when (exception is not OperationCanceledException && Translate(exception) is { } error)
        {
            // 业务异常是正常的失败分支：按异常自带的级别（默认 Information）记一条，不进错误日志
            logger.Log(exception is BusinessException business ? business.LogLevel : LogLevel.Information, exception,
                "Invocation '{TraceId}' failed with a business error '{Code}'.", context.TraceId, error.Code);
            context.SetOutcome(Result.Failure(error));
        }
    }

    /// <summary>
    /// 可翻译的异常给出错误，其余返回 null（交给内核按未预期处理）；派生类型先于基类型匹配
    /// </summary>
    private static Error? Translate(Exception exception)
    {
        return exception switch
        {
            ConcurrencyConflictException ex => Error.Literal(ConcurrencyCode, ex.Message, DetailsOf(null, ex.Details)),
            BusinessRuleValidationException ex => Error.Literal(DomainRuleCode, ex.Message, DetailsOf(ex.BrokenRule?.GetType().Name, ex.Details)),
            DomainException ex => Error.Literal(DomainRuleCode, ex.Message, DetailsOf(null, ex.Details)),
            BusinessException ex => Error.Literal(BusinessCode, ex.Message, DetailsOf(ex.Code, ex.Details)),
            UnauthorizedAccessException ex => Error.Literal(UnauthorizedCode, ex.Message),
            KeyNotFoundException ex => Error.Literal(NotFoundCode, ex.Message),
            ArgumentException ex => Error.Literal(ArgumentCode, ex.Message),
            InvalidOperationException ex => Error.Literal(InvalidOperationCode, ex.Message),
            _ => null,
        };
    }

    /// <summary>
    /// 业务码与补充说明进入错误明细；都没有时不带明细
    /// </summary>
    private static IReadOnlyDictionary<string, object?>? DetailsOf(string? code, string? details)
    {
        if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(details))
        {
            return null;
        }

        var map = new Dictionary<string, object?>(StringComparer.Ordinal);
        if (!string.IsNullOrWhiteSpace(code))
        {
            map["code"] = code;
        }

        if (!string.IsNullOrWhiteSpace(details))
        {
            map["details"] = details;
        }

        return map;
    }
}
