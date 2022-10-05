namespace DM.Services.Gaming.Dto.Shared;

/// <summary>
/// DTO model for a possible attribute list value
/// </summary>
public class ListValue
{
    /// <summary>
    /// Value
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Modifier
    /// </summary>
    public int? Modifier { get; set; }
}