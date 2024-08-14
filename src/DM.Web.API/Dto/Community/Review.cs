using System;
using DM.Web.API.BbRendering;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Dto.Community;

/// <summary>
/// API DTO model for site review
/// </summary>
public class Review
{
    /// <summary>
    /// Identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Review author
    /// </summary>
    public User Author { get; set; }

    /// <summary>
    /// Creating moment
    /// </summary>
    public DateTimeOffset Created { get; set; }

    /// <summary>
    /// Publish flag
    /// </summary>
    public bool? Approved { get; set; }

    /// <summary>
    /// Review text
    /// </summary>
    public CommonBbText Text { get; set; }
}