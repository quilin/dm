using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using DM.Services.Core.Dto;

namespace DM.Web.API.Binding;

/// <inheritdoc />
internal class OptionalConverterFactory : JsonConverterFactory
{
    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Optional<>);

    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var optionalType = typeToConvert.GetGenericArguments()[0];
        var converter = (JsonConverter) Activator.CreateInstance(
            typeof(OptionalConverter<>).MakeGenericType(optionalType),
            BindingFlags.Instance | BindingFlags.Public,
            null, new object[0], null);
        return converter;
    }
}

/// <inheritdoc />
internal class OptionalConverter<TValue> : JsonConverter<Optional<TValue>> where TValue : struct
{
    /// <inheritdoc />
    public override Optional<TValue> Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        var readerValue = reader.GetString();
        if (readerValue == null)
        {
            return null;
        }

        var nullableValue = JsonSerializer.Deserialize<TValue>(ref reader, options);
        return Optional<TValue>.WithValue(nullableValue);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Optional<TValue> value, JsonSerializerOptions options)
    {
        var resultValue = value.Value;
        JsonSerializer.Serialize(writer, resultValue, options);
    }
}