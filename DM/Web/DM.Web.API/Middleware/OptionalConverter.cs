using System;
using System.Linq;
using DM.Services.Core.Dto;
using Newtonsoft.Json;

namespace DM.Web.API.Middleware
{
    /// <inheritdoc />
    public class OptionalConverter : JsonConverter
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var resultValue = value.GetType().GetProperties().First(p => p.Name == nameof(Optional<int>.Value))
                .GetValue(value);
            serializer.Serialize(writer, resultValue);
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            var optionalValueType = objectType.GetGenericArguments().First();
            var nullableValueType = typeof(Nullable<>).MakeGenericType(optionalValueType);
            var nullableValue = serializer.Deserialize(reader, nullableValueType);
            return objectType.GetMethod(nameof(Optional<int>.WithValue))?.Invoke(null, new[] {nullableValue});
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType) =>
            objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Optional<>);
    }
}