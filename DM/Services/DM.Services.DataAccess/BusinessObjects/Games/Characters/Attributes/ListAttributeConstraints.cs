using System.Collections.Generic;
using System.Linq;

namespace DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes;

/// <summary>
/// DAL model for attribute constraints for a list of limited values
/// </summary>
public class ListAttributeConstraints : AttributeConstraints
{
    /// <summary>
    /// Possible values
    /// </summary>
    public IEnumerable<ListAttributeValue> Values { get; set; }

    /// <inheritdoc />
    public override string GetDefaultValue() => Values?.FirstOrDefault()?.Value ?? string.Empty;
}

/// <summary>
/// DAL model for a possible list value
/// </summary>
public class ListAttributeValue
{
    /// <summary>
    /// Value
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Modifier for the value
    /// </summary>
    public int? Modifier { get; set; }
}