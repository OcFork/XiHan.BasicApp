// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace XiHan.BasicApp.Core.Serialization;

/// <summary>
/// <see cref="DateTimeOffset"/>：写 ISO 8601（带偏移），读 ISO 8601 以及管理端日期控件给出的 <c>yyyy-MM-dd HH:mm:ss</c> / <c>yyyy-MM-dd</c>
/// （不带偏移时按服务器本地时区解释，与 4.x 一致）；不认识的文本报错，不悄悄变成默认值
/// </summary>
public sealed class LenientDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    /// <inheritdoc />
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"The value is not a valid date and time (token '{reader.TokenType}').");
        }

        // 标准格式走 STJ 自己的解析，其余（空格分隔的墙钟、只有日期）按不变区域性宽松解析
        if (reader.TryGetDateTimeOffset(out var value))
        {
            return value;
        }

        var text = reader.GetString();
        return DateTimeOffset.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal, out value)
            ? value
            : throw new JsonException($"The value '{text}' is not a valid date and time.");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        writer.WriteStringValue(value);
    }
}
