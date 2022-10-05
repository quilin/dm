using System;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Chat.Reading;

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
    public DateTimeOffset CreateDate { get; set; }

    /// <summary>
    /// Message author
    /// </summary>
    public GeneralUser Author { get; set; }

    /// <summary>
    /// Content
    /// </summary>
    public string Text { get; set; }
}