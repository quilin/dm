using System;
using System.Collections.Generic;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Dto;
using DM.Web.API.BbRendering;
using DM.Web.API.Dto.Games.Attributes;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Dto.Games;

/// <summary>
/// API DTO model for game
/// </summary>
public class Game
{
    /// <summary>
    /// Game identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Game title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// RPG system name
    /// </summary>
    public string System { get; set; }

    /// <summary>
    /// RPG setting name
    /// </summary>
    public string Setting { get; set; }

    /// <summary>
    /// Attribute schema identifier
    /// </summary>
    public AttributeSchema Schema { get; set; }

    /// <summary>
    /// Game status
    /// </summary>
    public GameStatus? Status { get; set; }

    /// <summary>
    /// Game first release date
    /// </summary>
    public DateTimeOffset? Released { get; set; }

    /// <summary>
    /// Game master
    /// </summary>
    public User Master { get; set; }

    /// <summary>
    /// Game master's assistant
    /// </summary>
    public User Assistant { get; set; }

    /// <summary>
    /// Responsible for premoderation
    /// </summary>
    public User Nanny { get; set; }

    /// <summary>
    /// Game master's pending assistant
    /// </summary>
    public User PendingAssistant { get; set; }

    /// <summary>
    /// Requesting user participates in game
    /// </summary>
    public IEnumerable<GameParticipation> Participation { get; set; }

    /// <summary>
    /// Game tags
    /// </summary>
    public IEnumerable<Tag> Tags { get; set; }

    /// <summary>
    /// Game information
    /// </summary>
    public InfoBbText Info { get; set; }

    /// <summary>
    /// Game private notes for master
    /// </summary>
    public string Notes { get; set; }

    /// <summary>
    /// Game privacy settings
    /// </summary>
    public GamePrivacySettings PrivacySettings { get; set; }

    /// <summary>
    /// Number of unread posts
    /// </summary>
    public int UnreadPostsCount { get; set; }

    /// <summary>
    /// Number of unread commentaries
    /// </summary>
    public int UnreadCommentsCount { get; set; }

    /// <summary>
    /// Number of unread characters
    /// </summary>
    public int UnreadCharactersCount { get; set; }
}

/// <summary>
/// DTO model for game privacy settings
/// </summary>
public class GamePrivacySettings
{
    /// <summary>
    /// User can read characters temper
    /// </summary>
    public bool? ViewTemper { get; set; }

    /// <summary>
    /// User can read characters story
    /// </summary>
    public bool? ViewStory { get; set; }

    /// <summary>
    /// User can read characters skills
    /// </summary>
    public bool? ViewSkills { get; set; }

    /// <summary>
    /// User can read characters inventory
    /// </summary>
    public bool? ViewInventory { get; set; }

    /// <summary>
    /// User can read other players private messages in in-game posts
    /// </summary>
    public bool? ViewPrivates { get; set; }

    /// <summary>
    /// User can see other players dice rolls and results
    /// </summary>
    public bool? ViewDice { get; set; }

    /// <summary>
    /// Access mode to the game commentaries
    /// </summary>
    public CommentariesAccessMode? CommentariesAccess { get; set; }
}