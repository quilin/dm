namespace DM.Web.API.Dto.Games.Attributes;

/// <summary>
/// DTO model for possible attribute value
/// </summary>
public class AttributeValueSpecification
{
    /// <summary>
    /// Value
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Value modifier
    /// </summary>
    public int? Modifier { get; set; }
}