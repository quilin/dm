using System;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Gaming.Dto.Input;

/// <summary>
/// DTO for new room claim
/// </summary>
public class CreateRoomClaim
{
    /// <summary>
    /// Room identifier
    /// </summary>
    public Guid RoomId { get; set; }

    /// <summary>
    /// Character identifier
    /// </summary>
    public Guid? CharacterId { get; set; }

    /// <summary>
    /// Reader login
    /// </summary>
    public string ReaderLogin { get; set; }

    /// <summary>
    /// Access policy
    /// </summary>
    public RoomAccessPolicy Policy { get; set; }
}