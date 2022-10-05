namespace DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes;

/// <summary>
/// DAL model for attribute constraints for a string
/// </summary>
public class StringAttributeConstraints : AttributeConstraints
{
    /// <summary>
    /// Maximum string length
    /// </summary>
    public int MaxLength { get; set; }

    /// <inheritdoc />
    public override string GetDefaultValue() => string.Empty;
}