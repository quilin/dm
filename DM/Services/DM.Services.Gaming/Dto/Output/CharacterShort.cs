using System;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Gaming.Dto.Output;

/// <summary>
/// Shortened character info
/// </summary>
public class CharacterShort
{
    /// <summary>
    /// Identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Character owner
    /// </summary>
    public GeneralUser Author { get; set; }

    /// <summary>
    /// Character status
    /// </summary>
    public CharacterStatus Status { get; set; }

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
    /// Picture URL
    /// </summary>
    public string PictureUrl { get; set; }

    /// <summary>
    /// Character is NPC
    /// </summary>
    public bool IsNpc { get; set; }

    /// <summary>
    /// GM access policy
    /// </summary>
    public CharacterAccessPolicy AccessPolicy { get; set; }
}