using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BBCodeParser;
using BBCodeParser.Tags;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DM.Web.Classic.Extensions.HtmlHelperExtensions
{
    public static class TextInputExtensions
    {
        private const string TextAreaClassName = "smartTextArea";

        public static async Task<IHtmlContent> SmartTextAreaFor<TModel, TValue>(this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, IBbParser parser,
            string placeholder = null, IDictionary<string, object> inputHtmlAttributes = null,
            IDictionary<string, object> placeholderHtmlAttributes = null,
            IDictionary<string, object> placeholderWrapperHtmlAttributes = null,
            IDictionary<string, object> inputWrapperHtmlAttributes = null)
        {
            inputHtmlAttributes = inputHtmlAttributes ?? new Dictionary<string, object>();

            if (inputHtmlAttributes.ContainsKey("class"))
            {
                inputHtmlAttributes["class"] += $" {TextAreaClassName}";
            }
            else
            {
                inputHtmlAttributes.Add("class", TextAreaClassName);
            }

            string textAreaId;
            if (!inputHtmlAttributes.ContainsKey("id"))
            {
                textAreaId = html.IdFor(expression);
            }
            else
            {
                textAreaId = (string) inputHtmlAttributes["id"];
            }

            var smartTextAreaViewModel = new SmartTextAreaViewModel
            {
                Tags = parser.GetTags()
                    .Where(t => ControlRenderers.ContainsKey(t.Name))
                    .Select(CreateControl)
                    .ToArray(),
                TextAreaId = textAreaId,
                TextArea = html.TextAreaFor(expression, placeholder, inputHtmlAttributes, placeholderHtmlAttributes,
                    placeholderWrapperHtmlAttributes, inputWrapperHtmlAttributes),
                ValidationMessage = html.ValidationMessageFor(expression)
            };
            return await html.PartialAsync("~/Views/Shared/SmartTextArea/SmartTextArea.cshtml", smartTextAreaViewModel);
        }

        public static Task<IHtmlContent> SmartTextAreaFor<TModel, TValue>(this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, IBbParser parser,
            string placeholder, object htmlAttributes)
        {
            return SmartTextAreaFor(html, expression, parser, placeholder,
                HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        private static readonly Dictionary<string, Func<IHtmlHelper, IHtmlContent>> ControlRenderers =
            new Dictionary<string, Func<IHtmlHelper, IHtmlContent>>
            {
                {"b", helper => new HtmlString("<strong>b</strong>")},
                {"i", helper => new HtmlString("<em>i</em>")},
                {"u", helper => new HtmlString("<u>u</u>")},
                {"s", helper => new HtmlString("<s>s</s>")},

                {"tab", helper => IconFont.Render(IconType.Tab)},
                {"head", helper => new HtmlString("<strong>H</strong>")},
                {"code", helper => IconFont.Render(IconType.Code)},
                {"spoiler", helper => IconFont.Render(IconType.Spoiler)},
                {"ul", helper => IconFont.Render(IconType.UnorderedList)},
                {"ol", helper => IconFont.Render(IconType.OrderedList)},
                {"img", helper => IconFont.Render(IconType.Image)},
                {"link", helper => IconFont.Render(IconType.Link)},
                {"quote", helper => IconFont.Render(IconType.Quote)},
                {"private", helper => IconFont.Render(IconType.Private)},
                {"pre", helper => new HtmlString("<span class=\"smartTextArea-control-pre\">p</span>")}
            };

        private static SmartTextAreaControlViewModel CreateControl(Tag tag)
        {
            return new SmartTextAreaControlViewModel
            {
                TagInfo = tag,
                RenderContent = ControlRenderers[tag.Name]
            };
        }
    }
}