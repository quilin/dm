using System;
using DM.Services.Core.Dto.Enums;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Dto.Games;

/// <summary>
/// API DTO model for room claim
/// </summary>
public class RoomClaim
{
    /// <summary>
    /// Claim identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Access policy
    /// </summary>
    public RoomAccessPolicy? Policy { get; set; }

    /// <summary>
    /// Character
    /// </summary>
    public Character Character { get; set; }

    /// <summary>
    /// Reader or character author
    /// </summary>
    public User User { get; set; }
}