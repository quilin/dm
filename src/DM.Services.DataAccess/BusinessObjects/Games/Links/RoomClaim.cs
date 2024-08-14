using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Games.Characters;
using DM.Services.DataAccess.BusinessObjects.Games.Posts;

namespace DM.Services.DataAccess.BusinessObjects.Games.Links;

/// <summary>
/// DAL model for character room link
/// </summary>
public class RoomClaim
{
    /// <summary>
    /// Link identifier
    /// </summary>
    [Key]
    public Guid RoomClaimId { get; set; }

    /// <summary>
    /// Participant identifier
    /// </summary>
    public Guid ParticipantId { get; set; }

    /// <summary>
    /// Room identifier
    /// </summary>
    public Guid RoomId { get; set; }

    /// <summary>
    /// Access policy
    /// </summary>
    public RoomAccessPolicy Policy { get; set; }

    /// <summary>
    /// Character
    /// </summary>
    [ForeignKey(nameof(ParticipantId))]
    public virtual Character Character { get; set; }

    /// <summary>
    /// Reader
    /// </summary>
    [ForeignKey(nameof(ParticipantId))]
    public virtual Reader Reader { get; set; }

    /// <summary>
    /// Room
    /// </summary>
    [ForeignKey(nameof(RoomId))]
    public virtual Room Room { get; set; }
}