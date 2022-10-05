namespace DM.Services.Gaming.Authorization;

/// <summary>
/// List of attribute schema actions that requires authorization
/// </summary>
public enum AttributeSchemaIntention
{
    /// <summary>
    /// Edit existing schema
    /// </summary>
    Edit = 0,

    /// <summary>
    /// Delete existing schema
    /// </summary>
    Delete = 1,

    /// <summary>
    /// Use schema in a game
    /// </summary>
    Use = 2
}