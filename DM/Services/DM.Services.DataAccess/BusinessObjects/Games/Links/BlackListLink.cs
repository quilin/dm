using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Games.Links;

/// <summary>
/// DAL model or blacklist link
/// </summary>
[Table("BlackListLinks")]
public class BlackListLink
{
    /// <summary>
    /// Link identifier
    /// </summary>
    [Key]
    public Guid BlackListLinkId { get; set; }

    /// <summary>
    /// Game identifier
    /// </summary>
    public Guid GameId { get; set; }

    /// <summary>
    /// Blacklisted user identifier
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Game
    /// </summary>
    [ForeignKey(nameof(GameId))]
    public virtual Game Game { get; set; }

    /// <summary>
    /// Blacklisted user
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }
}