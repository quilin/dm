using System.Collections.Generic;
using DM.Services.Core.Dto;
using DM.Services.Gaming.Dto.Shared;

namespace DM.Services.Gaming.Dto.Output;

/// <summary>
/// Extended DTO model for game
/// </summary>
public class GameExtended : Game
{
    /// <summary>
    /// Game readers
    /// </summary>
    public IEnumerable<GeneralUser> Readers { get; set; }

    /// <summary>
    /// Game characters
    /// </summary>
    public IEnumerable<CharacterShortInfo> Characters { get; set; }

    /// <summary>
    /// Game public information
    /// </summary>
    public string Info { get; set; }

    /// <summary>
    /// Game private master information
    /// </summary>
    public string Notepad { get; set; }

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
    /// Only GM and post author can see dice roll result
    /// </summary>
    public bool HideDiceResult { get; set; }

    /// <summary>
    /// Disable character alignment
    /// </summary>
    public bool DisableAlignment { get; set; }

    /// <summary>
    /// Any user can read private messages within posts
    /// </summary>
    public bool ShowPrivateMessages { get; set; }

    /// <summary>
    /// Attribute schema details
    /// </summary>
    public AttributeSchema AttributeSchema { get; set; }
}