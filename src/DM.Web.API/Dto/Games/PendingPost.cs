using System;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Dto.Games;

/// <summary>
/// DTO model for post anticipation
/// </summary>
public class PendingPost
{
    /// <summary>
    /// Identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// User who waits a post
    /// </summary>
    public User Awaiting { get; set; }

    /// <summary>
    /// User who is being awaited
    /// </summary>
    public User Pending { get; set; }

    /// <summary>
    /// Date since the post is being awaited
    /// </summary>
    public DateTimeOffset WaitsSince { get; set; }
}