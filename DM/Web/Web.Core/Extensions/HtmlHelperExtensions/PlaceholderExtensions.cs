using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DM.Web.Core.Extensions.HtmlHelperExtensions
{
    public static class PlaceholderExtensions
    {
        private const string PlaceholderClass = "placeholder";
        private const string PlaceholderWrapperClass = "placeholder-wrapper";
        private const string InputWrapperClass = "placeholder-input-wrapper";
        private const string JsPlaceholderClass = "js-placeholder";

        private static IHtmlContent PlaceholderHelper(IHtmlContent inputHtmlString, string placeholder,
            IDictionary<string, object> placeholderHtmlAttributes,
            IDictionary<string, object> placeholderWrapperHtmlAttributes,
            IDictionary<string, object> inputWrapperHtmlAttributes)
        {
            var placeholderTag = new TagBuilder("span");
            placeholderTag.InnerHtml.Append(placeholder);
            placeholderTag.MergeAttributes(placeholderHtmlAttributes);
            placeholderTag.AddCssClass(PlaceholderClass);

            var placeholderWrapperTag = new TagBuilder("span");
            placeholderWrapperTag.InnerHtml.AppendHtml(placeholderTag);
            placeholderWrapperTag.MergeAttributes(placeholderWrapperHtmlAttributes);
            placeholderWrapperTag.AddCssClass(PlaceholderWrapperClass);

            var inputWrapperTag = new TagBuilder("span");
            inputWrapperTag.MergeAttributes(inputWrapperHtmlAttributes);
            inputWrapperTag.AddCssClass(InputWrapperClass);

            inputWrapperTag.InnerHtml.AppendHtml(placeholderWrapperTag);
            inputWrapperTag.InnerHtml.AppendHtml(inputHtmlString);

            return new HtmlContentBuilder().AppendHtml(inputWrapperTag);
        }

        private static IDictionary<string, object> ModifyInputHtmlAttributes(IDictionary<string, object> inputHtmlAttributes)
        {
            if (inputHtmlAttributes == null)
            {
                inputHtmlAttributes = new Dictionary<string, object>();
            }

            if (!inputHtmlAttributes.ContainsKey("class"))
            {
                inputHtmlAttributes.Add("class", JsPlaceholderClass);
            }
            else
            {
                inputHtmlAttributes["class"] += $" {JsPlaceholderClass}";
            }

            return inputHtmlAttributes;
        }

        public static IHtmlContent TextAreaFor<TModel, TValue>(this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, string placeholder,
            IDictionary<string, object> inputHtmlAttributes = null,
            IDictionary<string, object> placeholderHtmlAttributes = null,
            IDictionary<string, object> placeholderWrapperHtmlAttributes = null,
            IDictionary<string, object> inputWrapperHtmlAttributes = null)
        {
            var modifiedInputHtmlAttributes = ModifyInputHtmlAttributes(inputHtmlAttributes);
            var inputHtmlString = html.TextAreaFor(expression, modifiedInputHtmlAttributes);

            return PlaceholderHelper(inputHtmlString, placeholder, placeholderHtmlAttributes,
                                     placeholderWrapperHtmlAttributes, inputWrapperHtmlAttributes);
        }

        public static IHtmlContent TextBoxFor<TModel, TValue>(this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, string placeholder,
            IDictionary<string, object> inputHtmlAttributes = null,
            IDictionary<string, object> placeholderHtmlAttributes = null,
            IDictionary<string, object> placeholderWrapperHtmlAttributes = null,
            IDictionary<string, object> inputWrapperHtmlAttributes = null)
        {
            var modifiedInputHtmlAttributes = ModifyInputHtmlAttributes(inputHtmlAttributes);
            var inputHtmlString = html.TextBoxFor(expression, modifiedInputHtmlAttributes);

            return PlaceholderHelper(inputHtmlString, placeholder, placeholderHtmlAttributes,
                                     placeholderWrapperHtmlAttributes, inputWrapperHtmlAttributes);
        }

        public static IHtmlContent TextBoxWithPlaceholder<TModel>(this IHtmlHelper<TModel> html,
            object value, string placeholder,
            IDictionary<string, object> inputHtmlAttributes = null,
            IDictionary<string, object> placeholderHtmlAttributes = null,
            IDictionary<string, object> placeholderWrapperHtmlAttributes = null,
            IDictionary<string, object> inputWrapperHtmlAttributes = null)
        {
            var modifiedInputHtmlAttributes = ModifyInputHtmlAttributes(inputHtmlAttributes);
            var inputTagBuilder = new TagBuilder("input") {TagRenderMode = TagRenderMode.SelfClosing};
            inputTagBuilder.Attributes.Add("type", "text");
            if (value != null)
            {
                inputTagBuilder.Attributes.Add("value", value.ToString());
            }
            inputTagBuilder.MergeAttributes(modifiedInputHtmlAttributes);
            var inputHtmlString = new HtmlContentBuilder().AppendHtml(inputTagBuilder);

            return PlaceholderHelper(inputHtmlString, placeholder, placeholderHtmlAttributes,
                                     placeholderWrapperHtmlAttributes, inputWrapperHtmlAttributes);
        }
    }
}