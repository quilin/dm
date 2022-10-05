using System;
using DM.Services.Core.Dto.Enums;
using DM.Web.API.BbRendering;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Dto.Games;

/// <summary>
/// DTO model for post vote
/// </summary>
public class Vote
{
    /// <summary>
    /// Target post
    /// </summary>
    public Post Post { get; set; }

    /// <summary>
    /// Vote author
    /// </summary>
    public User Author { get; set; }

    /// <summary>
    /// Vote sign
    /// </summary>
    public VoteSign Sign { get; set; }

    /// <summary>
    /// Vote reason type
    /// </summary>
    public VoteType Type { get; set; }

    /// <summary>
    /// Vote text
    /// </summary>
    public CommonBbText Text { get; set; }

    /// <summary>
    /// Creation moment
    /// </summary>
    public DateTimeOffset Created { get; set; }
}