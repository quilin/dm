using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.DataContracts;
using DM.Services.DataAccess.BusinessObjects.Games.Links;

namespace DM.Services.DataAccess.BusinessObjects.Games.Posts;

/// <summary>
/// DAL model for game room
/// </summary>
[Table("Rooms")]
public class Room : IRemovable
{
    /// <summary>
    /// Room identifier
    /// </summary>
    [Key]
    public Guid RoomId { get; set; }

    /// <summary>
    /// Game identifier
    /// </summary>
    public Guid GameId { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Room access type
    /// </summary>
    public RoomAccessType AccessType { get; set; }

    /// <summary>
    /// Room type (default/chat)
    /// </summary>
    public RoomType Type { get; set; }

    /// <summary>
    /// Display order number
    /// </summary>
    public double OrderNumber { get; set; }

    /// <summary>
    /// Previous room identifier (for 2-linked-list)
    /// </summary>
    public Guid? PreviousRoomId { get; set; }

    /// <summary>
    /// Next room identifier (for 2-linked-list)
    /// </summary>
    public Guid? NextRoomId { get; set; }

    /// <inheritdoc />
    public bool IsRemoved { get; set; }

    /// <summary>
    /// Game
    /// </summary>
    [ForeignKey(nameof(GameId))]
    public virtual Game Game { get; set; }

    /// <summary>
    /// Previous room (for 2-linked-list)
    /// </summary>
    [ForeignKey(nameof(PreviousRoomId))]
    public virtual Room PreviousRoom { get; set; }

    /// <summary>
    /// Next room (for 2-linked-list)
    /// </summary>
    [ForeignKey(nameof(NextRoomId))]
    public virtual Room NextRoom { get; set; }

    /// <summary>
    /// Character access links
    /// </summary>
    [InverseProperty(nameof(RoomClaim.Room))]
    public virtual ICollection<RoomClaim> RoomClaims { get; set; }

    /// <summary>
    /// Posts
    /// </summary>
    [InverseProperty(nameof(Post.Room))]
    public virtual ICollection<Post> Posts { get; set; }

    /// <summary>
    /// Post anticipations in room
    /// </summary>
    [InverseProperty(nameof(PendingPost.Room))]
    public virtual ICollection<PendingPost> PendingPosts { get; set; }
}