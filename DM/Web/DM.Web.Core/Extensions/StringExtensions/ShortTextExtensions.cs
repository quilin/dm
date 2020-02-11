using System.Text.RegularExpressions;

namespace DM.Web.Core.Extensions.StringExtensions
{
    /// <summary>
    /// String extensions related to text display shortage
    /// </summary>
    public static class ShortTextExtensions
    {
        /// <summary>
        /// Shorten the text
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="maxSymbolsCount">Maximum result string length</param>
        /// <returns>Shorten text</returns>
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
            var lastWhitespaceRegex = new Regex(@"\s[^\>]$");
            input = lastWhitespaceRegex.Replace(input, string.Empty);

            return input + "...";
        }
    }
}