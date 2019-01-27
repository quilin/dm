using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Core.Extensions.IconFontExtensions
{
    public static class IconFont
    {
        private static readonly int[] BaseActionsCharCodes =
        {
            IconType.Edit, IconType.Close, IconType.Settings, IconType.Reorder, IconType.Remove, IconType.Warning,
            IconType.Ban
        };

        public static IHtmlContent Render(int charCode, IDictionary<string, object> htmlAttributes = null)
        {
            htmlAttributes = htmlAttributes ?? new Dictionary<string, object>();
            htmlAttributes.Add("unselectable", "on");

            var wrapperTag = new TagBuilder("span");
            wrapperTag.MergeAttributes(htmlAttributes);

            wrapperTag.AddCssClass("iconic");
            wrapperTag.AddCssClass("base-unselectable");
            if (BaseActionsCharCodes.Contains(charCode))
            {
                wrapperTag.AddCssClass("icon-base-action");
            }

            wrapperTag.InnerHtml.AppendHtml($"&#x{charCode:X4};");

            return new HtmlContentBuilder().AppendHtml(wrapperTag);
        }
    }
}