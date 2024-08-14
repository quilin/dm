using System;
using System.Linq;

namespace DM.Services.Mail.Sender;

/// <summary>
/// Extensions for address logging obfuscation
/// </summary>
public static class AddressObfuscationExtensions
{
    /// <summary>
    /// Obfuscate email address for logs
    /// </summary>
    /// <param name="emailAddress"></param>
    /// <returns></returns>
    public static string Obfuscate(this string emailAddress) =>
        string.Join('@',
            emailAddress.Split("@")
                .Select(CropMiddle));

    private static string CropMiddle(string input)
    {
        var firstPart = input.Substring(0, Math.Min(2, input.Length));
        var lastPart = input.Substring(input.Length - 1);
        return $"{firstPart}..{lastPart}";
    }
}