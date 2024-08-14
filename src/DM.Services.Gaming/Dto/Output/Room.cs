using System;
using System.Collections.Generic;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Gaming.Dto.Output;

/// <summary>
/// DTO Model for game room
/// </summary>
public class Room
{
    /// <summary>
    /// Identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Game identifier
    /// </summary>
    public Guid GameId { get; set; }

    /// <summary>
    /// Room title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Room content type
    /// </summary>
    public RoomType Type { get; set; }

    /// <summary>
    /// Room access type
    /// </summary>
    public RoomAccessType AccessType { get; set; }

    /// <summary>
    /// Room character and reader claims
    /// </summary>
    public IEnumerable<RoomClaim> Claims { get; set; }

    /// <summary>
    /// Post anticipations
    /// </summary>
    public IEnumerable<PendingPost> Pendings { get; set; }

    /// <summary>
    /// Total room posts count
    /// </summary>
    public int TotalPostsCount { get; set; }

    /// <summary>
    /// Unread posts count
    /// </summary>
    public int UnreadPostsCount { get; set; }

    /// <summary>
    /// Room order number
    /// </summary>
    public double OrderNumber { get; set; }

    /// <summary>
    /// Previous room identifier
    /// </summary>
    public Guid? PreviousRoomId { get; set; }

    /// <summary>
    /// Default room name
    /// </summary>
    public const string DefaultRoomName = "Основная комната";
}