using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DM.Web.Classic.Extensions.HtmlHelperExtensions
{
    public static class RadioButtonExtensions
    {
        public static IHtmlContent RadioButtonFor<TModel, TValue>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression, TValue value, bool disabled)
            where TModel : class
            where TValue : struct
        {
            var htmlAttributes = new Dictionary<string, object>();
            if (disabled)
            {
                htmlAttributes.Add("disabled", "disabled");
            }
            var defaultValue = (TValue)expression.Compile().DynamicInvoke(htmlHelper.ViewData.Model);
            if (value.Equals(defaultValue))
            {
                htmlAttributes.Add("checked", "checked");
            }

            return htmlHelper.RadioButtonFor(expression, value, htmlAttributes);
        }
    }
}