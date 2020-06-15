using System;
using System.Linq;
using DM.Services.Core.Parsing;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace DM.Web.API.BbRendering
{
    /// <inheritdoc />
    public class BbConverter : JsonConverter
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IBbParserProvider bbParserProvider;

        /// <inheritdoc />
        public BbConverter(
            IHttpContextAccessor httpContextAccessor,
            IBbParserProvider bbParserProvider)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.bbParserProvider = bbParserProvider;
        }

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object valueObject, JsonSerializer serializer)
        {
            var bbText = (BbText) valueObject;
            var httpContext = httpContextAccessor.HttpContext;
            var renderMode = httpContext.Request.Headers.TryGetValue("X-Dm-Bb-Render-Mode", out var headerValues) &&
                headerValues.Any() && Enum.TryParse<BbRenderMode>(headerValues.First(), out var requiredRenderMode)
                    ? requiredRenderMode
                    : BbRenderMode.Html;
            var value = bbText.Value;

            if (renderMode == BbRenderMode.SafeHtml)
            {
                writer.WriteValue(bbParserProvider.CurrentSafePost.Parse(value).ToHtml());
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

            writer.WriteValue(text);
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer) => reader.Value switch
        {
            string stringValue => new CommonBbText {Value = stringValue},
            BbText bbTextValue => bbTextValue,
            _ => throw new ArgumentOutOfRangeException()
        };

        /// <inheritdoc />
        public override bool CanConvert(Type objectType) => objectType.IsSubclassOf(typeof(BbText));
    }
}