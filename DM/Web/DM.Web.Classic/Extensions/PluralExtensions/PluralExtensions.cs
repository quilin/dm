using Microsoft.AspNetCore.Mvc.Rendering;

namespace DM.Web.Classic.Extensions.PluralExtensions
{
    public static class PluralExtensions
    {
        public static string Plural<TModel>(this IHtmlHelper<TModel> htmlHelper, int count, params string[] variants)
        {
            var firstNumber = count%10;
            var secondNumber = count%100/10;

            // Takes care of just 0 too
            if (firstNumber == 0)
            {
                return variants[2];
            }
            if (secondNumber == 1)
            {
                return variants[2];
            }
            if (firstNumber == 1)
            {
                return variants[0];
            }

            return firstNumber < 5
                ? variants[1]
                : variants[2];
        }
    }
}