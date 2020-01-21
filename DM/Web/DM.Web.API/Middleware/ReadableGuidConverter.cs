using System;
using DM.Services.Core.Extensions;
using Newtonsoft.Json;

namespace DM.Web.API.Middleware
{
    /// <summary>
    /// Converter for readable guid fields 
    /// </summary>
    public class ReadableGuidConverter : JsonConverter<Guid>
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, Guid value, JsonSerializer serializer) =>
            writer.WriteValue(value.EncodeToReadable());

        /// <inheritdoc />
        public override Guid ReadJson(JsonReader reader, Type objectType, Guid existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var readerValue = (string) reader.Value;
            return Guid.TryParse(readerValue, out var simpleGuid)
                ? simpleGuid
                : readerValue.TryDecodeFromReadableGuid(out var decodedGuid)
                    ? decodedGuid
                    : default;
        }
    }

    /// <summary>
    /// Converter for readable but nullable fields
    /// </summary>
    public class ReadableNullableGuidConverter : JsonConverter<Guid?>
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, Guid? value, JsonSerializer serializer)
        {
            if (value.HasValue)
            {
                writer.WriteValue(value.Value.EncodeToReadable());
            }
            else
            {
                writer.WriteNull();
            }
        }

        /// <inheritdoc />
        public override Guid? ReadJson(JsonReader reader, Type objectType, Guid? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var readerValue = (string) reader.Value;
            return Guid.TryParse(readerValue, out var simpleGuid)
                ? simpleGuid
                : readerValue.TryDecodeFromReadableGuid(out var decodedGuid)
                    ? decodedGuid
                    : default;
        }
    }
}