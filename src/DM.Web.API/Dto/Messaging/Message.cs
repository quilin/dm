using System;
using DM.Web.API.BbRendering;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Dto.Messaging;

/// <summary>
/// API DTO for private message
/// </summary>
public class Message
{
    /// <summary>
    /// Identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Creating moment
    /// </summary>
    public DateTimeOffset Created { get; set; }

    /// <summary>
    /// Message author
    /// </summary>
    public User Author { get; set; }

    /// <summary>
    /// Message content
    /// </summary>
    public CommonBbText Text { get; set; }
}