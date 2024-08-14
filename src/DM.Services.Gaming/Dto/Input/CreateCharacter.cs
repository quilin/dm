using System;
using System.Collections.Generic;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.Dto.Input;

/// <summary>
/// DTO model for new character
/// </summary>
public class CreateCharacter
{
    /// <summary>
    /// Game identifier
    /// </summary>
    public Guid GameId { get; set; }

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
    /// Character alignment
    /// </summary>
    public Alignment? Alignment { get; set; }

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
    /// Character is NPC
    /// </summary>
    public bool IsNpc { get; set; }

    /// <summary>
    /// Character access policy
    /// </summary>
    public CharacterAccessPolicy AccessPolicy { get; set; }

    /// <summary>
    /// Character attributes
    /// </summary>
    public IEnumerable<CharacterAttribute> Attributes { get; set; }
}