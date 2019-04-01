using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DM.Web.Core.Extensions.BoolExtensions
{
    /// <summary>
    /// Extensions for HTML helpers to display online flag for user
    /// </summary>
    public static class PresenceExtensions
    {
        private const string OnlineClassName = "userPresence-positive";
        private const string OfflineClassName = "userPresence-negative";

        /// <summary>
        /// Get HTML to display if user is online
        /// </summary>
        /// <param name="isOnline"></param>
        /// <returns></returns>
        public static IHtmlContent PresenceFormat(this bool isOnline)
        {
            var span = new TagBuilder("span");
            span.AddCssClass(isOnline ? OnlineClassName : OfflineClassName);
            span.InnerHtml.Append(isOnline ? "online" : "offline");

            return new HtmlContentBuilder().AppendHtml(span);
        }
    }
}