namespace DM.Services.Gaming.Authorization;

/// <summary>
/// List of character intentions that requires authorization
/// </summary>
public enum CharacterIntention
{
    /// <summary>
    /// Edit character general information
    /// </summary>
    Edit = 1,

    /// <summary>
    /// Edit character privacy settings
    /// </summary>
    EditPrivacySettings = 2,

    /// <summary>
    /// Edit character master settings
    /// </summary>
    EditMasterSettings = 3,

    /// <summary>
    /// Delete character
    /// </summary>
    Delete = 4,

    /// <summary>
    /// Accept the character
    /// </summary>
    Accept = 5,

    /// <summary>
    /// Decline the character
    /// </summary>
    Decline = 6,

    /// <summary>
    /// Kill the character
    /// </summary>
    Kill = 7,

    /// <summary>
    /// Resurrect the character
    /// </summary>
    Resurrect = 8,

    /// <summary>
    /// Leave the game
    /// </summary>
    Leave = 9,

    /// <summary>
    /// Return to the game
    /// </summary>
    Return = 10,

    /// <summary>
    /// View character temper
    /// </summary>
    ViewTemper = 11,

    /// <summary>
    /// View character story
    /// </summary>
    ViewStory = 12,

    /// <summary>
    /// View character skills
    /// </summary>
    ViewSkills = 13,

    /// <summary>
    /// View character inventory
    /// </summary>
    ViewInventory = 14
}