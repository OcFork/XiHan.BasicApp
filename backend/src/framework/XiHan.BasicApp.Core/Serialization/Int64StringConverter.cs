// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace XiHan.BasicApp.Core.Serialization;

/// <summary>
/// <see cref="long"/> 写成 JSON 字符串、读字符串或数字：主键是雪花 ID（超过 2^53），JavaScript 的 Number 存不下，
/// 网络契约上一律按字符串传（与 4.x 的 Web 序列化一致）
/// </summary>
public sealed class Int64StringConverter : JsonConverter<long>
{
    /// <inheritdoc />
    public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Number => reader.GetInt64(),
            JsonTokenType.String when long.TryParse(reader.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var value) => value,
            // 不认识的值不悄悄变成 0：让绑定按载荷不合法报错
            _ => throw new JsonException($"The value is not a valid 64-bit integer (token '{reader.TokenType}')."),
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        writer.WriteStringValue(value.ToString(CultureInfo.InvariantCulture));
    }
}
