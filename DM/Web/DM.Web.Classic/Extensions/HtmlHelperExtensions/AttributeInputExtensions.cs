using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes;
using DM.Web.Classic.Views.Shared.CharacterAttribute;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DM.Web.Classic.Extensions.HtmlHelperExtensions
{
    public static class AttributeInputExtensions
    {
        public static IHtmlContent AttributeInput<TModel>(this IHtmlHelper<TModel> htmlHelper,
            IDictionary<string, object> htmlAttributes = null) where TModel : CharacterAttributeViewModel
        {
            var characterAttribute = (CharacterAttributeViewModel)htmlHelper.ViewData.Model;
            if (characterAttribute.Constraints is ListAttributeConstraints listAttributeConstraints &&
                listAttributeConstraints.Values.Length > 0)
            {
                var listValuesIndex = listAttributeConstraints.Values.ToDictionary(v => v.Value, v => v.Modifier);

                KeyValuePair<string, int?> defaultValue;
                if (string.IsNullOrEmpty(characterAttribute.Value) ||
                    !listValuesIndex.ContainsKey(characterAttribute.Value))
                {
                    var firstConstraintValue = listAttributeConstraints.Values[0];
                    defaultValue = new KeyValuePair<string, int?>(firstConstraintValue.Value, firstConstraintValue.Modifier);
                }
                else
                {
                    defaultValue = new KeyValuePair<string, int?>(characterAttribute.Value, listValuesIndex[characterAttribute.Value]);
                }
                
                var constraintValues = listAttributeConstraints.Values.Select(v => new KeyValuePair<string, int?>(v.Value, v.Modifier)).ToArray();
                return htmlHelper.DropdownForAttributes(m => m.Value, constraintValues, defaultValue);
            }

            var constraintsAttributes = GetConstraintsAttributes(characterAttribute);
            return htmlHelper.TextBoxFor(m => m.Value, string.Empty, constraintsAttributes.Merge(htmlAttributes));
        }

        private static IDictionary<string, object> GetConstraintsAttributes(CharacterAttributeViewModel characterAttribute)
        {
            var attributeConstraints = characterAttribute.Constraints;
            var result = new Dictionary<string, object>
                             {
                                 {"data-val", "true"},
                                 {"autocomplete", "off"}
                             };
            if (attributeConstraints.Required)
            {
                result.Add("data-val-required", "Введите значение характеристики");
            }

            switch (attributeConstraints)
            {
                case StringAttributeConstraints stringAttributeConstraints:
                    result.Add("maxlength", stringAttributeConstraints.MaxLength);
                    return result;
                
                case NumberAttributeConstraints numberAttributeConstraints:
                    result.Add("size", numberAttributeConstraints.Size);
                    result.Add("data-val-regex", "Введите число");
                    result.Add("data-val-regex-pattern", @"^\-?\d+$");
                    if (numberAttributeConstraints.MinValue.HasValue || numberAttributeConstraints.MaxValue.HasValue)
                    {
                        result.Add("data-val-range",
                            string.Format(GetNumberConstraintsValidationMessage(numberAttributeConstraints),
                                characterAttribute.Name, numberAttributeConstraints.MinValue,
                                numberAttributeConstraints.MaxValue));
                        result.Add("data-val-range-min", numberAttributeConstraints.MinValue ?? int.MinValue);
                        result.Add("data-val-range-max", numberAttributeConstraints.MaxValue ?? int.MaxValue);
                    }
                    break;
            }

            return result;
        }

        private static string GetNumberConstraintsValidationMessage(NumberAttributeConstraints constraints)
        {
            var result = new StringBuilder("Значение характеристики \"{0}\" ");

            if (constraints.MinValue.HasValue && constraints.MaxValue.HasValue)
            {
                result.Append("должно лежать в диапазоне от {1} до {2}");
            }
            else if (constraints.MinValue.HasValue)
            {
                result.Append("не может быть меньше {1}");
            }
            else
            {
                result.Append("не может быть больше {2}");
            }

            return result.ToString();
        }

        private static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> dict,
            IDictionary<TKey, TValue> merged)
        {
            if (merged == null || merged.Count == 0)
            {
                return dict;
            }
            return dict.Keys.Union(merged.Keys).ToDictionary(k => k, k => dict.ContainsKey(k) ? dict[k] : merged[k]);
        } 
    }
}