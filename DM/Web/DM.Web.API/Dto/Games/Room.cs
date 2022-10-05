using System;
using System.Collections.Generic;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;

namespace DM.Web.API.Dto.Games;

/// <summary>
/// DTO model for game room
/// </summary>
public class Room
{
    /// <summary>
    /// Room identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Previous room identifier
    /// </summary>
    public Optional<Guid> PreviousRoomId { get; set; }

    /// <summary>
    /// Room title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Room access type
    /// </summary>
    public RoomAccessType? Access { get; set; }

    /// <summary>
    /// Room content type
    /// </summary>
    public RoomType? Type { get; set; }

    /// <summary>
    /// Room claims
    /// </summary>
    public IEnumerable<RoomClaim> Claims { get; set; }

    /// <summary>
    /// Post pendings
    /// </summary>
    public IEnumerable<PendingPost> Pendings { get; set; }

    /// <summary>
    /// Number of unread posts
    /// </summary>
    public int UnreadPostsCount { get; set; }
}