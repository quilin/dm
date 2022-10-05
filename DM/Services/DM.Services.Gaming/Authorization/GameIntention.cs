namespace DM.Services.Gaming.Authorization;

/// <summary>
/// List of game actions that requires authorization
/// </summary>
public enum GameIntention
{
    /// <summary>
    /// Create new game
    /// </summary>
    Create = 0,

    /// <summary>
    /// Read game details
    /// </summary>
    Read = 1,

    /// <summary>
    /// Edit game
    /// </summary>
    Edit = 2,

    /// <summary>
    /// Remove game
    /// </summary>
    Delete = 3,

    /// <summary>
    /// Move game on moderation
    /// </summary>
    SetStatusModeration = 4,

    /// <summary>
    /// Move game to draft
    /// </summary>
    SetStatusDraft = 5,

    /// <summary>
    /// Move game to requirement
    /// </summary>
    SetStatusRequirement = 6,

    /// <summary>
    /// Move game to active
    /// </summary>
    SetStatusActive = 7,

    /// <summary>
    /// Froze the game
    /// </summary>
    SetStatusFrozen = 8,

    /// <summary>
    /// Finish the game
    /// </summary>
    SetStatusFinished = 9,

    /// <summary>
    /// Close the game
    /// </summary>
    SetStatusClosed = 10,

    /// <summary>
    /// Read game commentaries
    /// </summary>
    ReadComments = 11,

    /// <summary>
    /// Post game commentaries
    /// </summary>
    CreateComment = 12,

    /// <summary>
    /// Subscribe to a game
    /// </summary>
    Subscribe = 13,

    /// <summary>
    /// Unsubscribe from a game
    /// </summary>
    Unsubscribe = 14,

    /// <summary>
    /// Create game character
    /// </summary>
    CreateCharacter = 15
}