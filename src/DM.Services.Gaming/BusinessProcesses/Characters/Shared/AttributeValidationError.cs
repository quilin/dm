using System;
using System.Collections.Generic;
using System.Linq;
using DM.Services.Core.Extensions;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Shared;

/// <summary>
/// Errors for attribute value validation
/// </summary>
public static class AttributeValidationError
{
    /// <summary>
    /// Value only should be present when game has schema
    /// </summary>
    public static string NoSchema => "Game has no schema, so no attributes needed";

    /// <summary>
    /// Value should be of one of game specifications
    /// </summary>
    public static string InvalidSpecification => "Value specification should be present in the game schema";

    /// <summary>
    /// Specification marks this field as a required, but it is missing
    /// </summary>
    public static string RequiredMissing => "Value for this specification is required";

    /// <summary>
    /// Specifications are missing entirely from the data
    /// </summary>
    /// <param name="specifications"></param>
    /// <returns></returns>
    public static string ManyRequiredMissing(IEnumerable<(Guid Id, string Title)> specifications) =>
        $"These specifications were not found, but required: {string.Join(", ", specifications.Select(s => $"{s.Id.EncodeToReadable()} ({s.Title})"))}";

    /// <summary>
    /// Number should be a valid number
    /// </summary>
    public static string NotANumber => "Value should be a number";

    /// <summary>
    /// Number should be in range
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static string NumberNotInRange(int? min, int? max) =>
        $"Value should be in range from {min?.ToString() ?? "-∞"} to {max?.ToString() ?? "+∞"}";

    /// <summary>
    /// Value should be a short enough string
    /// </summary>
    /// <param name="maxLength"></param>
    /// <returns></returns>
    public static string StringTooLong(int maxLength) => $"Value should be at most {maxLength} characters long";

    /// <summary>
    /// Value should be present in list
    /// </summary>
    /// <param name="possibleValues"></param>
    /// <returns></returns>
    public static string NotPresentInList(IEnumerable<string> possibleValues) =>
        $"Value should be on of following values: {string.Join(", ", possibleValues)}";
}