namespace DM.Services.Gaming.Authorization;

/// <summary>
/// Room actions that require authorization
/// </summary>
public enum RoomIntention
{
    /// <summary>
    /// Create new post
    /// </summary>
    CreatePost = 1,

    /// <summary>
    /// Create new post anticipation
    /// </summary>
    CreatePendingPost = 2,

    /// <summary>
    /// Delete post anticipation
    /// </summary>
    DeletePending = 3
}