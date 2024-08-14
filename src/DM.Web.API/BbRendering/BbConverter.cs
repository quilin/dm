using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using DM.Services.Core.Parsing;
using Microsoft.AspNetCore.Http;

namespace DM.Web.API.BbRendering;

/// <inheritdoc />
internal class BbConverterFactory : JsonConverterFactory
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IBbParserProvider bbParserProvider;

    /// <inheritdoc />
    public BbConverterFactory(
        IHttpContextAccessor httpContextAccessor,
        IBbParserProvider bbParserProvider)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.bbParserProvider = bbParserProvider;
    }

    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsSubclassOf(typeof(BbText));

    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converter = (JsonConverter) Activator.CreateInstance(
            typeof(BbConverter<>).MakeGenericType(typeToConvert),
            BindingFlags.Instance | BindingFlags.Public,
            null, new object[] {httpContextAccessor, bbParserProvider}, null);
        return converter;
    }

    private class BbConverter<TBbText>(
        IHttpContextAccessor httpContextAccessor,
        IBbParserProvider bbParserProvider)
        : JsonConverter<TBbText>
        where TBbText : BbText, new()
    {
        public override TBbText Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options) => new() {Value = reader.GetString()};

        public override void Write(Utf8JsonWriter writer, TBbText bbText, JsonSerializerOptions options)
        {
            var httpContext = httpContextAccessor.HttpContext;
            var renderMode = httpContext.Request.Headers.TryGetValue("X-Dm-Bb-Render-Mode", out var headerValues) &&
                             headerValues.Any() && Enum.TryParse<BbRenderMode>(headerValues.First(), out var requiredRenderMode)
                ? requiredRenderMode
                : BbRenderMode.Html;
            var value = bbText.Value;
                
            if (renderMode == BbRenderMode.SafeHtml)
            {
                writer.WriteStringValue(bbParserProvider.CurrentSafePost.Parse(value).ToHtml());
                return;
            }
                
            var parsedTree = bbText.ParseMode switch
            {
                BbParseMode.Common => bbParserProvider.CurrentCommon.Parse(value),
                BbParseMode.Info => bbParserProvider.CurrentInfo.Parse(value),
                BbParseMode.Post => bbParserProvider.CurrentPost.Parse(value),
                BbParseMode.Chat => bbParserProvider.CurrentGeneralChat.Parse(value),
                _ => throw new ArgumentOutOfRangeException(nameof(bbText.ParseMode))
            };
                
            var text = renderMode switch
            {
                BbRenderMode.Html => parsedTree.ToHtml(),
                BbRenderMode.Bb => parsedTree.ToBb(),
                BbRenderMode.Text => parsedTree.ToText(),
                _ => throw new ArgumentOutOfRangeException()
            };
                
            writer.WriteStringValue(text);
        }
    }
}