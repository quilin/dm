using System;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Gaming.Dto.Input;

/// <summary>
/// DTO model for room claim updating
/// </summary>
public class UpdateRoomClaim
{
    /// <summary>
    /// Claim identifier
    /// </summary>
    public Guid ClaimId { get; set; }

    /// <summary>
    /// Access policy
    /// </summary>
    public RoomAccessPolicy Policy { get; set; }
}