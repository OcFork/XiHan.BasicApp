// Copyright (c) 2021-Present XiHanFun and contributors.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace XiHan.BasicApp.Core.Serialization;

/// <summary>
/// <see cref="Nullable{T}"/> 的 <see cref="DateTime"/>：null 与空串读成 null，其余同 <see cref="LenientDateTimeConverter"/>
/// </summary>
public sealed class NullableLenientDateTimeConverter : JsonConverter<DateTime?>
{
    /// <summary>
    /// 非空值的转换
    /// </summary>
    private static readonly LenientDateTimeConverter _inner = new();

    /// <inheritdoc />
    public override bool HandleNull => true;

    /// <inheritdoc />
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null || (reader.TokenType == JsonTokenType.String && string.IsNullOrWhiteSpace(reader.GetString())))
        {
            return null;
        }

        return _inner.Read(ref reader, typeof(DateTime), options);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        if (value is { } time)
        {
            writer.WriteStringValue(time);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
