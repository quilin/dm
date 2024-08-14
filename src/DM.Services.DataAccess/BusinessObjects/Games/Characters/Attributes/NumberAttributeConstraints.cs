namespace DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes;

/// <summary>
/// DAL model for attribute constraints for a number in range
/// </summary>
public class NumberAttributeConstraints : AttributeConstraints
{
    /// <summary>
    /// Minimum possible value (no minimal value if null)
    /// </summary>
    public int? MinValue { get; set; }
    /// <summary>
    /// Maximum possible value (no maximal value if null)
    /// </summary>
    public int? MaxValue { get; set; }

    /// <inheritdoc />
    public override string GetDefaultValue() => (MinValue ?? MaxValue ?? 0).ToString();
}