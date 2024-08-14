namespace DM.Services.Gaming.Authorization;

/// <summary>
/// List of post actions that requires authorization
/// </summary>
public enum PostIntention
{
    /// <summary>
    /// Change post text
    /// </summary>
    EditText = 1,

    /// <summary>
    /// Change post character
    /// </summary>
    EditCharacter = 2,

    /// <summary>
    /// Change post master message
    /// </summary>
    EditMasterMessage = 3,

    /// <summary>
    /// Delete post
    /// </summary>
    Delete = 4
}