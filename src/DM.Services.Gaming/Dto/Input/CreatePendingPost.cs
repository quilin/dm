using System;

namespace DM.Services.Gaming.Dto.Input;

/// <summary>
/// DTO model for creating pending posts
/// </summary>
public class CreatePendingPost
{
    /// <summary>
    /// Room identifier
    /// </summary>
    public Guid RoomId { get; set; }

    /// <summary>
    /// Pending user login
    /// </summary>
    public string PendingUserLogin { get; set; }
}