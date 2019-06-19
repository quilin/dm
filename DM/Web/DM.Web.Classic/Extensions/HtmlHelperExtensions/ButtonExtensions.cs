using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DM.Web.Classic.Extensions.HtmlHelperExtensions
{
    public static class ButtonExtensions
    {
        public static IHtmlContent Submit(this IHtmlHelper html, string value, IDictionary<string, object> htmlAttributes)
        {
            return Button(value, htmlAttributes, ActionButtonType.Submit);
        }

        public static IHtmlContent Submit(this IHtmlHelper html, string value, object htmlAttributes = null)
        {
            return Submit(html, value, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static IHtmlContent Button(this IHtmlHelper html, string value, IDictionary<string, object> htmlAttributes)
        {
            return Button(value, htmlAttributes, ActionButtonType.Button);
        }

        private static IHtmlContent Button(string value,
            IDictionary<string, object> htmlAttributes, ActionButtonType buttonType)
        {
            var buttonTypeText = buttonType.ToString().ToLower();
            var inputTag = new TagBuilder("input") {TagRenderMode = TagRenderMode.SelfClosing};
            inputTag.Attributes.Add("type", buttonTypeText);
            inputTag.Attributes.Add("value", value);
            inputTag.MergeAttributes(htmlAttributes);
            return new HtmlContentBuilder().AppendHtml(inputTag);
        }

        private static IHtmlContent SmallButton(IDictionary<string, object> htmlAttributes, ActionButtonType buttonType)
        {
            if (htmlAttributes == null)
            {
                htmlAttributes = new Dictionary<string, object>();
            }
            if (!htmlAttributes.ContainsKey("class"))
            {
                htmlAttributes.Add("class", "tinyButton");
            }
            else
            {
                htmlAttributes["class"] += " tinyButton";
            }
            return Button("", htmlAttributes, buttonType);
        }

        public static IHtmlContent SmallSubmit(this IHtmlHelper html, object htmlAttributes = null)
        {
            return SmallButton(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), ActionButtonType.Submit);
        }

        public static IHtmlContent SmallSubmit(this IHtmlHelper html, Dictionary<string, object> htmlAttributes)
        {
            return SmallButton(htmlAttributes, ActionButtonType.Submit);
        }

        public static IHtmlContent SmallReset(this IHtmlHelper html, object htmlAttributes = null)
        {
            return SmallButton(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), ActionButtonType.Reset);
        }
    }
}