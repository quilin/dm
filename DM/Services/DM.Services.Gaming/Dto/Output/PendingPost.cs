using System;
using DM.Services.Core.Dto;

namespace DM.Services.Gaming.Dto.Output;

/// <summary>
/// Room post anticipation
/// </summary>
public class PendingPost
{
    /// <summary>
    /// Identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Room identifier
    /// </summary>
    public Guid RoomId { get; set; }

    /// <summary>
    /// Who waits for the post
    /// </summary>
    public GeneralUser AwaitingUser { get; set; }

    /// <summary>
    /// Who needs to write the post
    /// </summary>
    public GeneralUser PendingUser { get; set; }

    /// <summary>
    /// For how long
    /// </summary>
    public DateTimeOffset WaitsSince { get; set; }
}