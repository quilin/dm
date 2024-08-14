using System;
using System.Collections.Generic;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Messaging.Reading;

/// <summary>
/// Service DTO for conversation
/// </summary>
public class Conversation
{
    /// <summary>
    /// Conversation identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// List of conversation participants
    /// </summary>
    public IEnumerable<GeneralUser> Participants { get; set; }

    /// <summary>
    /// Last conversation message
    /// </summary>
    public Message LastMessage { get; set; }

    /// <summary>
    /// Number of unread messages
    /// </summary>
    public int UnreadMessagesCount { get; set; }

    /// <summary>
    /// Total number of messages
    /// </summary>
    public int TotalMessagesCount { get; set; }
}