using System;
using System.Collections.Generic;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Gaming.Dto.Output;

/// <summary>
/// DTO model for game character
/// </summary>
public class Character
{
    /// <summary>
    /// Character identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Game identifier
    /// </summary>
    public Guid GameId { get; set; }

    /// <summary>
    /// Created date
    /// </summary>
    public DateTimeOffset CreateDate { get; set; }

    /// <summary>
    /// Last updated date
    /// </summary>
    public DateTimeOffset? LastUpdateDate { get; set; }

    /// <summary>
    /// Character status
    /// </summary>
    public CharacterStatus Status { get; set; }

    /// <summary>
    /// Total characters posts count
    /// </summary>
    public int TotalPostsCount { get; set; }

    /// <summary>
    /// Character author
    /// </summary>
    public GeneralUser Author { get; set; }

    /// <summary>
    /// Character name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Character race
    /// </summary>
    public string Race { get; set; }

    /// <summary>
    /// Character class
    /// </summary>
    public string Class { get; set; }

    /// <summary>
    /// Character picture URL
    /// </summary>
    public string PictureUrl { get; set; }

    /// <summary>
    /// Character appearance
    /// </summary>
    public string Appearance { get; set; }

    /// <summary>
    /// Character temper
    /// </summary>
    public string Temper { get; set; }

    /// <summary>
    /// Character story
    /// </summary>
    public string Story { get; set; }

    /// <summary>
    /// Character skills
    /// </summary>
    public string Skills { get; set; }

    /// <summary>
    /// Character inventory
    /// </summary>
    public string Inventory { get; set; }

    /// <summary>
    /// Character alignment
    /// </summary>
    public Alignment? Alignment { get; set; }

    /// <summary>
    /// Character is NPC (non-player's character)
    /// </summary>
    public bool IsNpc { get; set; }

    /// <summary>
    /// GM access policy
    /// </summary>
    public CharacterAccessPolicy AccessPolicy { get; set; }

    /// <summary>
    /// Character attribute
    /// </summary>
    public IEnumerable<CharacterAttribute> Attributes { get; set; }
}