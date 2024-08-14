using System;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Gaming.Dto.Internal;

/// <summary>
/// Internal DTO model for character updating
/// </summary>
internal class CharacterToUpdate
{
    /// <summary>
    /// Character author identifier
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Character create date
    /// </summary>
    public DateTimeOffset CreateDate { get; set; }

    /// <summary>
    /// Character status
    /// </summary>
    public CharacterStatus Status { get; set; }

    /// <summary>
    /// Character is NPC
    /// </summary>
    public bool IsNpc { get; set; }

    /// <summary>
    /// Character access policy
    /// </summary>
    public CharacterAccessPolicy AccessPolicy { get; set; }

    /// <summary>
    /// Character game identifier
    /// </summary>
    public Guid GameId { get; set; }

    /// <summary>
    /// Character game master user identifier
    /// </summary>
    public Guid GameMasterId { get; set; }

    /// <summary>
    /// Character game assistant user identifier
    /// </summary>
    public Guid? GameAssistantId { get; set; }

    /// <summary>
    /// Character game status
    /// </summary>
    public GameStatus GameStatus { get; set; }
}