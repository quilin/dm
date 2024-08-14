using System;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Gaming.Dto.Output;

/// <summary>
/// DTO model for room claim
/// </summary>
public class RoomClaim
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
    /// Access policy
    /// </summary>
    public RoomAccessPolicy Policy { get; set; }

    /// <summary>
    /// Character
    /// </summary>
    public Character Character { get; set; }

    /// <summary>
    /// Reader or character author
    /// </summary>
    public GeneralUser User { get; set; }
}