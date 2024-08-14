using System;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Messaging.Reading;

/// <summary>
/// Service DTO of message
/// </summary>
public class Message
{
    /// <summary>
    /// Message identifier
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
    /// Message content
    /// </summary>
    public string Text { get; set; }
}