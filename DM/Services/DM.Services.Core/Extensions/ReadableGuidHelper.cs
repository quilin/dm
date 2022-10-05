using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DM.Services.Core.Extensions;

/// <summary>
/// Utils to encode Guid into more readable format
/// </summary>
public static class ReadableGuidHelper
{
    /// <summary>
    /// Encode Guid to readable format
    /// </summary>
    /// <param name="guid">Guid</param>
    /// <param name="readableText">Custom text</param>
    /// <returns>Readable guid</returns>
    public static string EncodeToReadable(this Guid guid, string readableText = null)
    {
        var base64Guid = Convert.ToBase64String(guid.ToByteArray()).Replace("/", "-").Replace("+", "_")
            .Replace("=", "");
        return string.IsNullOrEmpty(readableText)
            ? base64Guid
            : $"{Transliterate(readableText.ToLower())}~{base64Guid}";
    }

    /// <summary>
    /// Decode guid from readable format
    /// </summary>
    /// <param name="encodedGuid">Encoded guid</param>
    /// <returns>Guid</returns>
    public static Guid DecodeFromReadableGuid(this string encodedGuid)
    {
        var base64Guid = encodedGuid.Split(new[] {"~"}, StringSplitOptions.None)
            .Last()
            .Replace("-", "/")
            .Replace("_", "+") + "==";
        return new Guid(Convert.FromBase64String(base64Guid));
    }

    /// <summary>
    /// Try decode guid from readable format
    /// </summary>
    /// <param name="encodedGuid">Encoded guid</param>
    /// <param name="result">Guid</param>
    /// <returns>Can be decoded</returns>
    public static bool TryDecodeFromReadableGuid(this string encodedGuid, out Guid result)
    {
        try
        {
            result = DecodeFromReadableGuid(encodedGuid);
            return true;
        }
        catch (Exception)
        {
            result = Guid.Empty;
            return false;
        }
    }

    private static string Transliterate(string input)
    {
        var result = new StringBuilder();
        foreach (var c in input.ToCharArray())
        {
            if (TransliterationReplacements.TryGetValue(c.ToString(CultureInfo.InvariantCulture),
                    out var replacement))
            {
                result.Append(replacement);
            }
        }

        return result.ToString();
    }

    private static readonly Dictionary<string, string> TransliterationReplacements = new()
    {
        {" ", "-"}, {"а", "a"}, {"б", "b"}, {"в", "v"}, {"г", "g"}, {"д", "d"}, {"е", "e"}, {"ё", "yo"},
        {"ж", "zh"}, {"з", "z"}, {"и", "i"}, {"й", "y"}, {"к", "k"}, {"л", "l"}, {"м", "m"}, {"н", "n"},
        {"о", "o"}, {"п", "p"}, {"р", "r"}, {"с", "s"}, {"т", "t"}, {"у", "u"}, {"ф", "f"}, {"х", "h"}, {"ц", "c"},
        {"ч", "ch"}, {"ш", "sh"}, {"щ", "tsh"}, {"ъ", ""}, {"ы", "y"}, {"ь", ""}, {"э", "e"},
        {"ю", "yu"}, {"я", "ya"}, {"a", "a"}, {"b", "b"}, {"c", "c"}, {"d", "d"}, {"e", "e"}, {"f", "f"},
        {"g", "g"}, {"h", "h"}, {"i", "i"}, {"j", "j"}, {"k", "k"}, {"l", "l"}, {"m", "m"}, {"n", "n"},
        {"o", "o"}, {"p", "p"}, {"q", "q"}, {"r", "r"}, {"s", "s"}, {"t", "t"}, {"u", "u"}, {"v", "v"}, {"w", "w"},
        {"x", "x"}, {"y", "y"}, {"z", "z"}, {"0", "0"}, {"1", "1"}, {"2", "2"}, {"3", "3"},
        {"4", "4"}, {"5", "5"}, {"6", "6"}, {"7", "7"}, {"8", "8"}, {"9", "9"}
    };
}