using System.Collections.Generic;
using System.Linq;

namespace DM.Services.Mail.Rendering.Rendering;

/// <summary>
/// Some constants for email templates
/// </summary>
public static class EmailConstants
{
    /// <summary>
    /// Email layout name
    /// </summary>
    public static string Layout => "_EmailLayout";

    /// <summary>
    /// Basic paragraph style
    /// </summary>
    public static string ParagraphStyles => "margin: 16px 0;";

    /// <summary>
    /// Basic button style
    /// </summary>
    public static string ButtonStyles => new Dictionary<string, string>
    {
        ["display"] = "block",
        ["width"] = "300px",
        ["margin"] = "0 auto",
        ["padding"] = "12px",
        ["border-radius"] = "4px",
        ["background-color"] = "#36a",
        ["color"] = "#fff",
        ["font-size"] = "18px",
        ["text-decoration"] = "none",
        ["text-align"] = "center"
    }.ConvertStyles();

    private static string ConvertStyles(this IDictionary<string, string> properties) =>
        string.Join("; ", properties.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
}