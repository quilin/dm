namespace DM.Services.Forum.Authorization;

/// <summary>
/// List of forum comment actions that requires authorization
/// </summary>
public enum CommentIntention
{
    /// <summary>
    /// Edit commentary
    /// </summary>
    Edit = 1,

    /// <summary>
    /// Remove commentary
    /// </summary>
    Delete = 2,

    /// <summary>
    /// Like commentary
    /// </summary>
    Like = 3
}