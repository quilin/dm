using System.Text.RegularExpressions;

namespace Web.Core.Extensions.StringExtensions
{
    public static class ShortTextExtensions
    {
        public static string Shorten(this string input, int maxSymbolsCount)
        {
            input = input.Trim();

            if (input.Length <= maxSymbolsCount)
            {
                return input;
            }

            input = input.Substring(0, maxSymbolsCount);

            var breakLineErrorRegex = new Regex(@"(\s*(\<|(\<b)|(\<br)|(\<br\s)|(\<br\s\/)|(\<br\s\/\>))\s*)+$");
            input = breakLineErrorRegex.Replace(input, string.Empty);
            var lastWhitespaceRegex = new Regex(@"\s[!\>]$");
            input = lastWhitespaceRegex.Replace(input, string.Empty);

            return input + "...";
        }
    }
}