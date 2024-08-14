using System;
using System.Collections.Generic;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Dto.Messaging;

/// <summary>
/// API DTO model for user conversation
/// </summary>
public class Conversation
{
    /// <summary>
    /// Identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Conversation name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Conversation participants
    /// </summary>
    public IEnumerable<User> Participants { get; set; }

    /// <summary>
    /// Last conversation message
    /// </summary>
    public Message LastMessage { get; set; }

    /// <summary>
    /// Number of unread conversation messages
    /// </summary>
    public int UnreadMessagesCount { get; set; }
}