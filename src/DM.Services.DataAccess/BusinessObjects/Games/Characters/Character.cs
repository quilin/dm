using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.DataContracts;
using DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.DataAccess.BusinessObjects.Games.Posts;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Games.Characters;

/// <summary>
/// DAL model for character
/// </summary>
[Table("Characters")]
public class Character : IRemovable
{
    /// <summary>
    /// Character identifier
    /// </summary>
    [Key]
    public Guid CharacterId { get; set; }

    /// <summary>
    /// Game identifier
    /// </summary>
    public Guid GameId { get; set; }

    /// <summary>
    /// Author identifier
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public CharacterStatus Status { get; set; }

    /// <summary>
    /// Creation moment
    /// </summary>
    public DateTimeOffset CreateDate { get; set; }

    /// <summary>
    /// Last update moment
    /// </summary>
    public DateTimeOffset? LastUpdateDate { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Race (e.g. elf, asari, human)
    /// </summary>
    public string Race { get; set; }

    /// <summary>
    /// Class (e.g. wizard, sniper)
    /// </summary>
    public string Class { get; set; }

    /// <summary>
    /// Old-school-D&amp;D stuff
    /// </summary>
    public Alignment? Alignment { get; set; }

    /// <summary>
    /// Appearance
    /// </summary>
    public string Appearance { get; set; }

    /// <summary>
    /// Temper
    /// </summary>
    public string Temper { get; set; }

    /// <summary>
    /// Life story
    /// </summary>
    public string Story { get; set; }

    /// <summary>
    /// Skills
    /// </summary>
    public string Skills { get; set; }

    /// <summary>
    /// Inventory
    /// </summary>
    public string Inventory { get; set; }

    /// <summary>
    /// NPC flag
    /// </summary>
    public bool IsNpc { get; set; }

    /// <summary>
    /// GM access policy
    /// </summary>
    public CharacterAccessPolicy AccessPolicy { get; set; }

    /// <inheritdoc />
    public bool IsRemoved { get; set; }

    /// <summary>
    /// Game
    /// </summary>
    [ForeignKey(nameof(GameId))]
    public virtual Game Game { get; set; }

    /// <summary>
    /// Author
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public virtual User Author { get; set; }

    /// <summary>
    /// Portrait
    /// </summary>
    [InverseProperty(nameof(Upload.Character))]
    public virtual ICollection<Upload> Pictures { get; set; }

    /// <summary>
    /// Attribute values
    /// </summary>
    [InverseProperty(nameof(CharacterAttribute.Character))]
    public virtual ICollection<CharacterAttribute> Attributes { get; set; }

    /// <summary>
    /// Room access
    /// </summary>
    [InverseProperty(nameof(RoomClaim.Character))]
    public virtual ICollection<RoomClaim> RoomLinks { get; set; }

    /// <summary>
    /// Posts
    /// </summary>
    [InverseProperty(nameof(Post.Character))]
    public virtual ICollection<Post> Posts { get; set; }
}