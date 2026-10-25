// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace XiHan.BasicApp.Core.Serialization;

/// <summary>
/// <see cref="DateTime"/>：写 ISO 8601，读 ISO 8601 以及 <c>yyyy-MM-dd HH:mm:ss</c> / <c>yyyy-MM-dd</c>（按不变区域性）；不认识的文本报错
/// </summary>
public sealed class LenientDateTimeConverter : JsonConverter<DateTime>
{
    /// <inheritdoc />
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"The value is not a valid date and time (token '{reader.TokenType}').");
        }

        if (reader.TryGetDateTime(out var value))
        {
            return value;
        }

        var text = reader.GetString();
        return DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out value)
            ? value
            : throw new JsonException($"The value '{text}' is not a valid date and time.");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        writer.WriteStringValue(value);
    }
}
