// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace XiHan.BasicApp.Core.Serialization;

/// <summary>
/// <see cref="Nullable{T}"/> 的 <see cref="long"/> 写成 JSON 字符串、读字符串或数字；null 与空串读成 null（见 <see cref="Int64StringConverter"/>）
/// </summary>
public sealed class NullableInt64StringConverter : JsonConverter<long?>
{
    /// <inheritdoc />
    public override bool HandleNull => true;

    /// <inheritdoc />
    public override long? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return null;
            case JsonTokenType.Number:
                return reader.GetInt64();
            case JsonTokenType.String:
                var text = reader.GetString();
                // 表单里没填的可选主键会以空串上送，按「没有」处理
                if (string.IsNullOrWhiteSpace(text))
                {
                    return null;
                }

                return long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value)
                    ? value
                    : throw new JsonException($"The value '{text}' is not a valid 64-bit integer.");
            default:
                throw new JsonException($"The value is not a valid 64-bit integer (token '{reader.TokenType}').");
        }
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, long? value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        if (value is { } number)
        {
            writer.WriteStringValue(number.ToString(CultureInfo.InvariantCulture));
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
