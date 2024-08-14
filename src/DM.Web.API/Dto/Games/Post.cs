using System;
using System.Collections.Generic;
using DM.Web.API.BbRendering;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Dto.Games;

/// <summary>
/// DTO model for game post
/// </summary>
public class Post
{
    /// <summary>
    /// Post identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Parent room
    /// </summary>
    public Room Room { get; set; }

    /// <summary>
    /// Post character
    /// </summary>
    public Character Character { get; set; }

    /// <summary>
    /// Post author
    /// </summary>
    public User Author { get; set; }

    /// <summary>
    /// Creation moment
    /// </summary>
    public DateTimeOffset Created { get; set; }

    /// <summary>
    /// Last update moment
    /// </summary>
    public DateTimeOffset? Updated { get; set; }

    /// <summary>
    /// Text
    /// </summary>
    public PostBbText Text { get; set; }

    /// <summary>
    /// Additional text
    /// </summary>
    public CommonBbText Commentary { get; set; }

    /// <summary>
    /// Private text to master
    /// </summary>
    public CommonBbText MasterMessage { get; set; }

    /// <summary>
    /// Dice roll results
    /// </summary>
    public IEnumerable<DiceRoll> DiceRolls { get; set; }
}