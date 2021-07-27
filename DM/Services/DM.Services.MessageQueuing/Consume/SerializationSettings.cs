using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace DM.Services.MessageQueuing.Consume
{
    internal static class SerializationSettings
    {
        public static readonly JsonSerializerOptions ForMessage = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };
    }
}