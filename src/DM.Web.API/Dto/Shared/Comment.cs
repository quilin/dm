using System;
using System.Collections.Generic;
using DM.Web.API.BbRendering;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Dto.Shared;

/// <summary>
/// API DTO model for commentary
/// </summary>
public class Comment
{
    /// <summary>
    /// Commentary identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Author
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
    public CommonBbText Text { get; set; }

    /// <summary>
    /// Users who liked it
    /// </summary>
    public IEnumerable<User> Likes { get; set; }
}