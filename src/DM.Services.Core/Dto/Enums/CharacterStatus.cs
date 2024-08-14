namespace DM.Services.Core.Dto.Enums;

/// <summary>
/// Character status
/// </summary>
public enum CharacterStatus
{
    /// <summary>
    /// The character requires GM review
    /// </summary>
    Registration = 0,

    /// <summary>
    /// GM declined the character
    /// </summary>
    Declined = 1,

    /// <summary>
    /// GM accepted the character and it is currently in the game
    /// </summary>
    Active = 2,

    /// <summary>
    /// GM killed the character or forced the player to leave
    /// </summary>
    Dead = 3,

    /// <summary>
    /// Player left the game voluntarily
    /// </summary>
    Left = 4
}