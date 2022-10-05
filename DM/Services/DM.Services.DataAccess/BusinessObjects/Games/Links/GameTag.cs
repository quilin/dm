using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Common;

namespace DM.Services.DataAccess.BusinessObjects.Games.Links;

/// <summary>
/// DAL model for game tag link
/// </summary>
[Table("GameTags")]
public class GameTag
{
    /// <summary>
    /// Link identifier
    /// </summary>
    [Key]
    public Guid GameTagId { get; set; }

    /// <summary>
    /// Game identifier
    /// </summary>
    public Guid GameId { get; set; }

    /// <summary>
    /// Tag identifier
    /// </summary>
    public Guid TagId { get; set; }

    /// <summary>
    /// Game
    /// </summary>
    [ForeignKey(nameof(GameId))]
    public virtual Game Game { get; set; }

    /// <summary>
    /// Tag
    /// </summary>
    [ForeignKey(nameof(TagId))]
    public virtual Tag Tag { get; set; }
}