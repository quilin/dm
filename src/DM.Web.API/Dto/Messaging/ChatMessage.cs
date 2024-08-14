using System;
using DM.Web.API.BbRendering;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Dto.Messaging;

/// <summary>
/// DTO model for chat message
/// </summary>
public class ChatMessage
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
    /// Author
    /// </summary>
    public User Author { get; set; }

    /// <summary>
    /// Content
    /// </summary>
    public ChatBbText Text { get; set; }
}