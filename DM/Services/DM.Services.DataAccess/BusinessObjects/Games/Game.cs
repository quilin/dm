using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.DataContracts;
using DM.Services.DataAccess.BusinessObjects.Games.Characters;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.DataAccess.BusinessObjects.Games.Posts;
using DM.Services.DataAccess.BusinessObjects.Games.Rating;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Games;

/// <summary>
/// DAL model for game
/// </summary>
[Table("Games")]
public class Game : IRemovable
{
    /// <summary>
    /// Game identifier
    /// </summary>
    [Key]
    public Guid GameId { get; set; }

    /// <summary>
    /// Creation moment
    /// </summary>
    public DateTimeOffset CreateDate { get; set; }

    /// <summary>
    /// Release moment (first time the game started requirement)
    /// </summary>
    public DateTimeOffset? ReleaseDate { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public GameStatus Status { get; set; }

    /// <summary>
    /// GM identifier
    /// </summary>
    public Guid MasterId { get; set; }

    /// <summary>
    /// GM assistant identifier
    /// </summary>
    public Guid? AssistantId { get; set; }

    /// <summary>
    /// Premoderation assistant identifier
    /// </summary>
    public Guid? NannyId { get; set; }

    /// <summary>
    /// Character attribute schema identifier
    /// </summary>
    public Guid? AttributeSchemaId { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// System name (e.g. D&amp;D, WoD)
    /// </summary>
    public string SystemName { get; set; }

    /// <summary>
    /// Setting name (e.g. Mass Effect, WarHammer, Our world)
    /// </summary>
    public string SettingName { get; set; }

    /// <summary>
    /// Full game information
    /// </summary>
    public string Info { get; set; }

    /// <summary>
    /// Only GM and character author can see character temper
    /// </summary>
    public bool HideTemper { get; set; }

    /// <summary>
    /// Only GM and character author can see character skills
    /// </summary>
    public bool HideSkills { get; set; }

    /// <summary>
    /// Only GM and character author can see character inventory
    /// </summary>
    public bool HideInventory { get; set; }

    /// <summary>
    /// Only GM and character author can see character story
    /// </summary>
    public bool HideStory { get; set; }

    /// <summary>
    /// Characters has no alignment
    /// </summary>
    public bool DisableAlignment { get; set; }

    /// <summary>
    /// Only GM and post author can see dice roll result
    /// </summary>
    public bool HideDiceResult { get; set; }

    /// <summary>
    /// Any user can read private messages within posts
    /// </summary>
    public bool ShowPrivateMessages { get; set; }

    /// <summary>
    /// Policy for game commentaries read/write access
    /// </summary>
    public CommentariesAccessMode CommentariesAccessMode { get; set; }

    /// <summary>
    /// Private notes for GM and assistant
    /// </summary>
    public string Notepad { get; set; }

    /// <inheritdoc />
    public bool IsRemoved { get; set; }

    /// <summary>
    /// GM
    /// </summary>
    [ForeignKey(nameof(MasterId))]
    public User Master { get; set; }

    /// <summary>
    /// GM assistant
    /// </summary>
    [ForeignKey(nameof(AssistantId))]
    public User Assistant { get; set; }

    /// <summary>
    /// Premoderation assistant
    /// </summary>
    [ForeignKey(nameof(NannyId))]
    public User Nanny { get; set; }

    /// <summary>
    /// Blacklist links
    /// </summary>
    [InverseProperty(nameof(BlackListLink.Game))]
    public virtual ICollection<BlackListLink> BlackList { get; set; }

    /// <summary>
    /// Game tags
    /// </summary>
    [InverseProperty(nameof(GameTag.Game))]
    public virtual ICollection<GameTag> GameTags { get; set; }

    /// <summary>
    /// Readers
    /// </summary>
    [InverseProperty(nameof(Reader.Game))]
    public virtual ICollection<Reader> Readers { get; set; }

    /// <summary>
    /// Characters
    /// </summary>
    [InverseProperty(nameof(Character.Game))]
    public virtual ICollection<Character> Characters { get; set; }

    /// <summary>
    /// Rooms
    /// </summary>
    [InverseProperty(nameof(Room.Game))]
    public virtual ICollection<Room> Rooms { get; set; }

    /// <summary>
    /// Votes for game posts
    /// </summary>
    [InverseProperty(nameof(Vote.Game))]
    public virtual ICollection<Vote> Votes { get; set; }

    /// <summary>
    /// Commentaries
    /// </summary>
    [InverseProperty(nameof(Comment.Game))]
    public virtual ICollection<Comment> Comments { get; set; }

    /// <summary>
    /// Game preview picture
    /// </summary>
    [InverseProperty(nameof(Upload.Game))]
    public virtual ICollection<Upload> Pictures { get; set; }

    /// <summary>
    /// Game authorization tokens
    /// </summary>
    [InverseProperty(nameof(Token.Game))]
    public virtual ICollection<Token> Tokens { get; set; }
}