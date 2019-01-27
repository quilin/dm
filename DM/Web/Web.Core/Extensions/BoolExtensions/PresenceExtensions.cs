using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Core.Extensions.BoolExtensions
{
    public static class PresenceExtensions
    {
        private const string OnlineClassName = "userPresence-positive";
        private const string OfflineClassName = "userPresence-negative";

        public static IHtmlContent PresenceFormat(this bool isOnline)
        {
            var span = new TagBuilder("span");
            span.AddCssClass(isOnline ? OnlineClassName : OfflineClassName);
            span.InnerHtml.Append(isOnline ? "online" : "offline");

            return new HtmlContentBuilder().AppendHtml(span);
        }
    }
}