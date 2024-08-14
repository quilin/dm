using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using DM.Services.Core.Extensions;

namespace DM.Web.API.Binding;

/// <summary>
/// Converter for readable guid fields 
/// </summary>
internal class ReadableGuidConverter : JsonConverter<Guid>
{
    /// <inheritdoc />
    public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var readerValue = reader.GetString();
        return Guid.TryParse(readerValue, out var simpleGuid)
            ? simpleGuid
            : readerValue.TryDecodeFromReadableGuid(out var decodedGuid)
                ? decodedGuid
                : default;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.EncodeToReadable());
}

/// <summary>
/// Converter for readable but nullable fields
/// </summary>
internal class ReadableNullableGuidConverter : JsonConverter<Guid?>
{
    /// <inheritdoc />
    public override Guid? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var readerValue = reader.GetString();
        return Guid.TryParse(readerValue, out var simpleGuid)
            ? simpleGuid
            : readerValue.TryDecodeFromReadableGuid(out var decodedGuid)
                ? decodedGuid
                : null;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Guid? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteStringValue(value.Value.EncodeToReadable());
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}