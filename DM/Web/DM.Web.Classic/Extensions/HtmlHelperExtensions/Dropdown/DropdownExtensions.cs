using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DM.Web.Core.Extensions.EnumExtensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DM.Web.Classic.Extensions.HtmlHelperExtensions.Dropdown
{
    public static class DropdownExtensions
    {
        private const string WrapperClassName = "dds-select";
        private const string SelectInputClassName = "dds-select-input";
        private const string AdditionalDataClassName = "dds-option-additionalData";
        private const string OptionsClassName = "dds-options";
        private const string OptionLinkClassName = "dds-option-link";
        private const string OptionGroupClassName = "dds-option-group";
        private const string OptionGroupTitleClassName = "dds-option-group-title";
        private const string DisabledOptionLinkClassName = "dds-option-link-disabled";
        private const string OptionWrapperClassName = "dds-option";
        private const string AttributesOptionsWrapperClassName = "dds-options-attributes";

        public static IDisposable BeginSuggest(this IHtmlHelper htmlHelper, string id, object value,
            IDictionary<string, object> optionsHtmlAttributes = null,
            IDictionary<string, object> inputHtmlAttributes = null,
            bool disabled = false)
        {
            return new DisposableDropdown(
                htmlHelper.ViewContext,
                () => BeginSuggest(id, value, disabled, optionsHtmlAttributes, inputHtmlAttributes),
                EndDropdownSelect);
        }

        public static IDisposable BeginSuggestFor<TModel, TValue>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression,
            IDictionary<string, object> optionsHtmlAttributes = null,
            IDictionary<string, object> inputHtmlAttributes = null,
            bool disabled = false)
        {
            return new DisposableDropdown(
                htmlHelper.ViewContext,
                () => BeginSuggestFor(htmlHelper, expression, disabled, optionsHtmlAttributes, inputHtmlAttributes),
                EndDropdownSelect);
        }

        public static IHtmlContent SuggestFor<TModel, TValue>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression, IEnumerable<TValue> values,
            IDictionary<string, object> inputHtmlAttributes,
            bool disabled = false)
        {
            var result = new HtmlContentBuilder()
                .AppendHtml(htmlHelper.TextBoxFor(expression,
                    ModifySuggestInputHtmlAttributes(inputHtmlAttributes, disabled)))
                .AppendHtml(BeginOptionsWrapper(htmlHelper.IdFor(expression), null));

            var dropdownables = values.Select(v => new Dropdownable(v.ToString()));
            return GenerateDropdown(htmlHelper, result, dropdownables);
        }

        public static IDisposable BeginDropdown(this IHtmlHelper htmlHelper, string id, object defaultValue,
            IDictionary<string, object> wrapperHtmlAttributes = null,
            IDictionary<string, object> optionsWrapperHtmlAttributes = null,
            string additionalData = null, bool disabled = false)
        {
            return new DisposableDropdown(
                htmlHelper.ViewContext,
                () => BeginDropdown(htmlHelper, id, null, wrapperHtmlAttributes, optionsWrapperHtmlAttributes,
                    defaultValue, additionalData, disabled, false),
                EndDropdownSelect
            );
        }

        public static IDisposable BeginDropdownFor<TModel, TValue>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression, string defaultCaption,
            IDictionary<string, object> wrapperHtmlAttributes = null,
            IDictionary<string, object> optionsHtmlAttributes = null,
            IDictionary<string, object> inputHtmlAttributes = null,
            bool disabled = false, string additionalData = null)
        {
            return new DisposableDropdown(
                htmlHelper.ViewContext,
                () => BeginDropdownFor(htmlHelper, expression, defaultCaption, disabled, wrapperHtmlAttributes,
                    optionsHtmlAttributes, inputHtmlAttributes, additionalData),
                EndDropdownSelect
            );
        }

        private static IHtmlContent BeginOptionsGroupWrapper(string value)
        {
            var groupWrapperTag = new TagBuilder("div") {TagRenderMode = TagRenderMode.StartTag};
            groupWrapperTag.AddCssClass(OptionGroupClassName);
            groupWrapperTag.InnerHtml.Append(value);

            var groupTitleTag = new TagBuilder("div");
            groupTitleTag.AddCssClass(OptionGroupTitleClassName);
            groupTitleTag.InnerHtml.Append(value);

            return new HtmlContentBuilder()
                .AppendHtml(groupWrapperTag)
                .AppendHtml(groupTitleTag);
        }

        private static IHtmlContent EndOptionsGroupWrapper()
        {
            return new TagBuilder("div") {TagRenderMode = TagRenderMode.EndTag};
        }

        private static IHtmlContent SelectOption(object value,
            IDictionary<string, object> optionHtmlAttributes,
            Func<object, IHtmlContent> createAdditionalData,
            object additionalData = null, bool disabled = false)
        {
            var linkTag = new TagBuilder("a");
            linkTag.Attributes.Add("href", "javascript:void(0)");
            linkTag.Attributes.Add("data-display", value.ToString());
            linkTag.MergeAttributes(optionHtmlAttributes);
            linkTag.AddCssClass(OptionLinkClassName);
            if (disabled)
            {
                linkTag.AddCssClass(DisabledOptionLinkClassName);
            }

            linkTag.InnerHtml.AppendHtml(new HtmlContentBuilder()
                .Append(value.ToString())
                .AppendHtml(createAdditionalData(additionalData)));

            var wrapperTag = new TagBuilder("div");
            wrapperTag.AddCssClass(OptionWrapperClassName);
            wrapperTag.InnerHtml.AppendHtml(linkTag);

            return wrapperTag;
        }

        public static IHtmlContent SelectOption(this IHtmlHelper htmlHelper, object value,
            IDictionary<string, object> optionHtmlAttributes, string additionalData = null, bool disabled = false)
        {
            return SelectOption(value, optionHtmlAttributes, d => CreateAdditionalData((string) d), additionalData,
                disabled);
        }

        private static IHtmlContent SelectAttributeOption(object value,
            IDictionary<string, object> optionHtmlAttributes, int? additionalData, bool disabled = false)
        {
            return SelectOption(value, optionHtmlAttributes, d => CreateAdditionalDataAsAttributeModifier((int?) d),
                additionalData, disabled);
        }


        public static IHtmlContent Dropdown<TModel>(this IHtmlHelper<TModel> htmlHelper, string name,
            IDictionary<string, object> values, object defaultValue, bool disabled = false,
            IDictionary<string, object> wrapperHtmlAttributes = null,
            IDictionary<string, object> optionsHtmlAttributes = null)
        {
            if (values.Count == 0)
            {
                throw new ArgumentException("Dropdown must contain values", nameof(values));
            }

            var groupedValues = values.ToDictionary(v => v.Key,
                v => (IDictionary<string, object>) new Dictionary<string, object> {{v.Key, v.Value}});

            return DropdownGroup(htmlHelper, name, groupedValues, defaultValue, disabled, wrapperHtmlAttributes,
                optionsHtmlAttributes);
        }

        public static IHtmlContent DropdownGroup<TModel>(this IHtmlHelper<TModel> htmlHelper,
            string name, IDictionary<string, IDictionary<string, object>> groupedValues, object defaultValue,
            bool disabled = false,
            IDictionary<string, object> wrapperHtmlAttributes = null,
            IDictionary<string, object> optionsHtmlAttributes = null)
        {
            if (groupedValues.Count == 0)
            {
                throw new ArgumentException("Dropdown must contain values", nameof(groupedValues));
            }

            var values = groupedValues.SelectMany(g => g.Value).ToDictionary(g => g.Key, g => g.Value);

            var result = new HtmlContentBuilder()
                .AppendHtml(CreateSelectWrapper(name, wrapperHtmlAttributes, defaultValue ?? values.Keys.First(), null,
                    disabled))
                .AppendHtml(CreateHiddenInput(htmlHelper, name, name, values.Values.First(), disabled))
                .AppendHtml(BeginOptionsWrapper(name, optionsHtmlAttributes));

            foreach (var groupedValue in groupedValues)
            {
                var group = groupedValue.Value;
                var needsGroupWrapper = group.Count > 1;
                if (needsGroupWrapper)
                {
                    result.AppendHtml(BeginOptionsGroupWrapper(groupedValue.Key));
                }

                foreach (var value in group)
                {
                    result.AppendHtml(SelectOption(htmlHelper, value.Key,
                        new Dictionary<string, object> {{"data-value", value.Value}}));
                }

                if (needsGroupWrapper)
                {
                    result.AppendHtml(EndOptionsGroupWrapper());
                }
            }

            result.AppendHtml(EndOptionsWrapper());

            return result;
        }

        public static IHtmlContent DropdownFor<TModel, TValue>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression, bool disabled = false,
            IDictionary<string, object> wrapperHtmlAttributes = null,
            IDictionary<string, object> optionsHtmlAttributes = null,
            IDictionary<string, object> inputHtmlAttributes = null)
            where TValue : struct
        {
            if (!typeof(TValue).IsEnum)
            {
                throw new ArgumentException($"{typeof(TValue)} should be enum", nameof(expression));
            }

            var metadata = ExpressionMetadataProvider.FromLambdaExpression(expression, htmlHelper.ViewData,
                htmlHelper.MetadataProvider);

            var values = Enum.GetValues(typeof(TValue))
                .Cast<TValue>()
                .Select(v => new Dropdownable(v.ToString(), (v as Enum).GetDescription()))
                .Cast<IDropdownable>();

            return DropdownFor(htmlHelper, expression, values, ((Enum) metadata.Model).GetDescription(), null, disabled,
                wrapperHtmlAttributes, optionsHtmlAttributes, inputHtmlAttributes);
        }

        public static IHtmlContent DropdownFor<TModel, TValue>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression, string[] values, string defaultValue,
            IDictionary<string, object> htmlAttributes, bool disabled = false)
        {
            if (values.Length == 0)
            {
                throw new ArgumentException("Dropdown must contain values", nameof(values));
            }

            if (values.Length == 1)
            {
                return new HtmlString(values[0]);
            }

            var dropdownableValues = values.Select(v => new Dropdownable(v));
            return DropdownFor(htmlHelper, expression, dropdownableValues, defaultValue, null, disabled, htmlAttributes,
                null, null);
        }

        public static IHtmlContent DropdownForAttributes<TModel, TValue>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression, KeyValuePair<string, int?>[] values,
            KeyValuePair<string, int?> defaultValue, bool disabled = false)
        {
            if (values.Length == 0)
            {
                throw new ArgumentException("Dropdown must contain values", nameof(values));
            }

            if (values.Length == 1)
            {
                var html = new HtmlContentBuilder().Append(values[0].Key);
                if (values[0].Value != null)
                {
                    html.AppendHtml(CreateAdditionalDataAsAttributeModifier(values[0].Value));
                }

                return html;
            }

            var htmlAttributes = new Dictionary<string, object> {{"class", AttributesOptionsWrapperClassName}};

            var dropdownableValues = values.Select(v => new Dropdownable(v.Key, v.Key, v.Value?.ToString()));
            return DropdownForAttributes(htmlHelper, expression, dropdownableValues, defaultValue.Key,
                defaultValue.Value, disabled, htmlAttributes, htmlAttributes);
        }

        public static IHtmlContent DropdownFor<TModel, TValue>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression, string defaultValue,
            IDictionary<TValue, string> descriptionsDictionary, bool disabled = false)
        {
            if (descriptionsDictionary == null)
            {
                throw new ArgumentException("Descriptions dictionary can not be null", nameof(descriptionsDictionary));
            }

            var values = descriptionsDictionary.Select(kvp => new Dropdownable(kvp.Key.ToString(), kvp.Value));
            return DropdownFor(htmlHelper, expression, values, defaultValue, null, disabled, null, null, null);
        }

        public static IHtmlContent DropdownFor<TModel, TValue>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression,
            IDictionary<TValue, string> descriptionsDictionary,
            IDictionary<string, object> wrapperHtmlAttributes = null,
            IDictionary<string, object> optionsHtmlAttributes = null,
            IDictionary<string, object> inputHtmlAttributes = null,
            bool disabled = false)
        {
            if (descriptionsDictionary == null)
            {
                throw new ArgumentException("Descriptions dictionary can not be null", nameof(descriptionsDictionary));
            }

            var metadata = ExpressionMetadataProvider.FromLambdaExpression(expression, htmlHelper.ViewData,
                htmlHelper.MetadataProvider);

            var values = descriptionsDictionary.Select(kvp => new Dropdownable(kvp.Key.ToString(), kvp.Value));
            return DropdownFor(htmlHelper, expression, values, descriptionsDictionary[(TValue) metadata.Model], null,
                disabled, wrapperHtmlAttributes, optionsHtmlAttributes, inputHtmlAttributes);
        }

        public static IHtmlContent DropdownFor<TModel, TValue>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression,
            IDictionary<TValue, string> descriptionsDictionary,
            IDictionary<TValue, string> additionalDataDictionary,
            IDictionary<string, object> wrapperHtmlAttributes = null,
            IDictionary<string, object> optionsHtmlAttributes = null,
            IDictionary<string, object> inputHtmlAttributes = null,
            bool disabled = false)
        {
            if (descriptionsDictionary == null)
            {
                throw new ArgumentException("Descriptions dictionary can not be null", nameof(descriptionsDictionary));
            }

            if (additionalDataDictionary == null)
            {
                throw new ArgumentException("Additional data dictionary can not be null",
                    nameof(additionalDataDictionary));
            }

            var metadata = ExpressionMetadataProvider.FromLambdaExpression(expression, htmlHelper.ViewData,
                htmlHelper.MetadataProvider);

            var values = descriptionsDictionary.Select(kvp => new Dropdownable(
                kvp.Key.ToString(), kvp.Value,
                additionalDataDictionary.TryGetValue(kvp.Key, out var value) ? value : null));
            return DropdownFor(htmlHelper, expression, values, descriptionsDictionary[(TValue) metadata.Model],
                null, disabled, wrapperHtmlAttributes, optionsHtmlAttributes, inputHtmlAttributes);
        }

        private static IHtmlContent DropdownFor<TModel, TValue>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression, IEnumerable<IDropdownable> values, string defaultValue,
            string defaultAdditionalData, bool disabled, IDictionary<string, object> wrapperHtmlAttributes,
            IDictionary<string, object> optionsHtmlAttributes, IDictionary<string, object> inputHtmlAttributes)
        {
            var id = htmlHelper.IdFor(expression);
            var result = new HtmlContentBuilder()
                .AppendHtml(CreateSelectWrapper(id, wrapperHtmlAttributes, defaultValue, defaultAdditionalData, disabled))
                .AppendHtml(CreateHiddenInputFor(htmlHelper, expression, id, disabled, inputHtmlAttributes))
                .AppendHtml(BeginOptionsWrapper(id, optionsHtmlAttributes));

            return GenerateDropdown(htmlHelper, result, values);
        }

        private static IHtmlContent DropdownForAttributes<TModel, TValue>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression, IEnumerable<IDropdownable> values, string defaultValue,
            int? defaultAdditionalData, bool disabled, IDictionary<string, object> wrapperHtmlAttributes,
            IDictionary<string, object> optionsHtmlAttributes)
        {
            var id = htmlHelper.IdFor(expression);
            var result = new HtmlContentBuilder()
                .AppendHtml(CreateAttributesSelectWrapper(id, wrapperHtmlAttributes, defaultValue, defaultAdditionalData,
                    disabled))
                .AppendHtml(CreateHiddenInputFor(htmlHelper, expression, id, disabled, null))
                .AppendHtml(BeginOptionsWrapper(id, optionsHtmlAttributes));

            return GenerateDropdownForAttributes(result, values);
        }

        private static IHtmlContent GenerateDropdown<TModel>(IHtmlHelper<TModel> htmlHelper, IHtmlContentBuilder result,
            IEnumerable<IDropdownable> values)
        {
            foreach (var value in values)
            {
                result.AppendHtml(SelectOption(htmlHelper, value.GetDescription(),
                    new Dictionary<string, object> {{"data-value", value.GetValue()}}, value.GetAdditionalData()));
            }

            result.AppendHtml(EndOptionsWrapper());

            return result;
        }

        private static IHtmlContent GenerateDropdownForAttributes(IHtmlContentBuilder result,
            IEnumerable<IDropdownable> values)
        {
            foreach (var value in values)
            {
                int? modifier = null;
                if (int.TryParse(value.GetAdditionalData(), out var additionalDataValue))
                {
                    modifier = additionalDataValue;
                }

                result.AppendHtml(SelectAttributeOption(value.GetDescription(),
                    new Dictionary<string, object> {{"data-value", value.GetValue()}}, modifier));
            }

            result.AppendHtml(EndOptionsWrapper());

            return result;
        }

        private static IHtmlContent BeginDropdown(IHtmlHelper htmlHelper, string id, string name,
            IDictionary<string, object> wrapperHtmlAttributes, IDictionary<string, object> optionsWrapperHtmlAttributes,
            object defaultValue, string additionalData, bool disabled, bool withHidden)
        {
            var result = new HtmlContentBuilder()
                .AppendHtml(CreateSelectWrapper(id, wrapperHtmlAttributes, defaultValue, additionalData, disabled))
                .AppendHtml(withHidden ? CreateHiddenInput(htmlHelper, id, name, defaultValue, disabled) : null)
                .AppendHtml(BeginOptionsWrapper(id, optionsWrapperHtmlAttributes));

            return result;
        }

        private static IHtmlContent BeginDropdownFor<TModel, TValue>(IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression, string defaultValue, bool disabled,
            IDictionary<string, object> wrapperHtmlAttributes,
            IDictionary<string, object> optionsHtmlAttributes,
            IDictionary<string, object> inputHtmlAttributes,
            string additionalData)
        {
            var id = htmlHelper.IdFor(expression);

            var result = new HtmlContentBuilder()
                .AppendHtml(CreateSelectWrapper(id, wrapperHtmlAttributes, defaultValue, additionalData, disabled))
                .AppendHtml(CreateHiddenInputFor(htmlHelper, expression, id, disabled, inputHtmlAttributes))
                .AppendHtml(BeginOptionsWrapper(id, optionsHtmlAttributes));

            return result;
        }

        private static IHtmlContent BeginSuggest(string id, object value, bool disabled,
            IDictionary<string, object> optionsHtmlAttributes, IDictionary<string, object> inputHtmlAttributes)
        {
            var result = new HtmlContentBuilder()
                .AppendHtml(CreateSuggestInput(id, value, disabled, inputHtmlAttributes))
                .AppendHtml(BeginOptionsWrapper(id, optionsHtmlAttributes));

            return result;
        }

        private static IHtmlContent BeginSuggestFor<TModel, TValue>(IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression, bool disabled,
            IDictionary<string, object> optionsHtmlAttributes,
            IDictionary<string, object> inputHtmlAttributes)
        {
            var id = htmlHelper.IdFor(expression);
            var value = ExpressionMetadataProvider.FromLambdaExpression(expression, htmlHelper.ViewData,
                htmlHelper.MetadataProvider).Model;

            var result = new HtmlContentBuilder()
                .AppendHtml(CreateSuggestInput(id, value, disabled, inputHtmlAttributes))
                .AppendHtml(BeginOptionsWrapper(id, optionsHtmlAttributes));

            return result;
        }

        private static IHtmlContent EndDropdownSelect()
        {
            return EndOptionsWrapper();
        }

        private static IHtmlContent CreateSelectWrapper(string id, IDictionary<string, object> wrapperHtmlAttributes,
            object defaultValue, string additionalData, bool disabled)
        {
            var inputTag = new TagBuilder("span");
            inputTag.AddCssClass(SelectInputClassName);
            inputTag.InnerHtml.AppendHtml(new HtmlContentBuilder()
                .AppendHtml(CreateAdditionalData(additionalData))
                .Append(defaultValue.ToString()));

            var wrapperTag = new TagBuilder("span");
            wrapperTag.MergeAttributes(wrapperHtmlAttributes);
            wrapperTag.Attributes["id"] = id;
            wrapperTag.Attributes["tabindex"] = "4";
            wrapperTag.AddCssClass(WrapperClassName);
            if (disabled)
                wrapperTag.Attributes["disabled"] = "disabled";
            wrapperTag.InnerHtml.AppendHtml(inputTag);

            return wrapperTag;
        }

        private static IHtmlContent CreateAttributesSelectWrapper(string id,
            IDictionary<string, object> wrapperHtmlAttributes,
            object defaultValue, int? additionalData, bool disabled)
        {
            var inputTag = new TagBuilder("span");
            inputTag.AddCssClass(SelectInputClassName);
            inputTag.InnerHtml.AppendHtml(new HtmlContentBuilder()
                .AppendHtml(CreateAdditionalDataAsAttributeModifier(additionalData))
                .Append(defaultValue.ToString()));

            var wrapperTag = new TagBuilder("span");
            wrapperTag.MergeAttributes(wrapperHtmlAttributes);
            wrapperTag.Attributes["id"] = id;
            wrapperTag.Attributes["tabindex"] = "4";
            wrapperTag.AddCssClass(WrapperClassName);
            if (disabled)
                wrapperTag.Attributes["disabled"] = "disabled";
            wrapperTag.InnerHtml.AppendHtml(inputTag);

            return wrapperTag;
        }

        private static IHtmlContent CreateHiddenInput(IHtmlHelper htmlHelper, string id, string name,
            object defaultValue, bool disabled)
        {
            var htmlAttributes = new Dictionary<string, object> {{"id", $"{id}_Hidden"}};
            if (disabled)
            {
                htmlAttributes.Add("disabled", "disabled");
            }

            return htmlHelper.Hidden(name, defaultValue, htmlAttributes);
        }

        private static IHtmlContent CreateHiddenInputFor<TModel, TValue>(IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression,
            string id, bool disabled, IDictionary<string, object> inputHtmlAttributes)
        {
            var htmlAttributes = inputHtmlAttributes ?? new Dictionary<string, object>();
            if (!htmlAttributes.ContainsKey("Id"))
            {
                htmlAttributes.Add("id", $"{id}_Hidden");
            }

            if (disabled)
            {
                htmlAttributes.Add("disabled", "disabled");
            }

            return htmlHelper.HiddenFor(expression, htmlAttributes);
        }

        private static IHtmlContent CreateAdditionalData(string additionalData,
            IDictionary<string, object> htmlAttributes = null)
        {
            if (string.IsNullOrEmpty(additionalData))
            {
                return new HtmlString(string.Empty);
            }

            var additionalDataTag = new TagBuilder("span");
            additionalDataTag.MergeAttributes(htmlAttributes);
            additionalDataTag.AddCssClass(AdditionalDataClassName);
            additionalDataTag.InnerHtml.Append(additionalData);

            return additionalDataTag;
        }

        private static IHtmlContent CreateAdditionalDataAsAttributeModifier(int? modifier)
        {
            if (!modifier.HasValue)
            {
                return null;
            }

            if (modifier.Value == 0)
            {
                return CreateAdditionalData("0");
            }

            if (modifier.Value > 0)
            {
                return CreateAdditionalData(string.Format($"+{modifier.Value}"),
                    new Dictionary<string, object> {{"class", "rating-positive"}});
            }

            return CreateAdditionalData(modifier.Value.ToString(),
                new Dictionary<string, object> {{"class", "rating-negative"}});
        }

        private static IHtmlContent BeginOptionsWrapper(string id,
            IDictionary<string, object> optionsWrapperHtmlAttributes)
        {
            var wrapperTag = new TagBuilder("div") {TagRenderMode = TagRenderMode.StartTag};
            wrapperTag.MergeAttributes(optionsWrapperHtmlAttributes);
            wrapperTag.Attributes["id"] = $"{id}_Options";
            wrapperTag.AddCssClass(OptionsClassName);

            return wrapperTag;
        }

        private static IHtmlContent EndOptionsWrapper()
        {
            return new TagBuilder("div") {TagRenderMode = TagRenderMode.EndTag};
        }

        private static IHtmlContent CreateSuggestInput(string id, object value, bool disabled,
            IDictionary<string, object> inputHtmlAttributes)
        {
            var inputTagBuilder = new TagBuilder("input") {TagRenderMode = TagRenderMode.SelfClosing};
            inputTagBuilder.Attributes.Add("type", "text");
            inputTagBuilder.Attributes.Add("id", id);
            if (value != null)
            {
                inputTagBuilder.Attributes.Add("value", value.ToString());
            }

            inputTagBuilder.MergeAttributes(ModifySuggestInputHtmlAttributes(inputHtmlAttributes, disabled));

            return inputTagBuilder;
        }

        private static IDictionary<string, object> ModifySuggestInputHtmlAttributes(
            IDictionary<string, object> htmlAttributes, bool disabled)
        {
            if (htmlAttributes.ContainsKey("class"))
            {
                htmlAttributes["class"] += " dds-suggest-input";
            }
            else
            {
                htmlAttributes.Add("class", "dds-suggest-input");
            }

            if (disabled)
            {
                htmlAttributes["disabled"] = "disabled";
            }

            htmlAttributes.Add("autocomplete", "off");
            return htmlAttributes;
        }
    }
}