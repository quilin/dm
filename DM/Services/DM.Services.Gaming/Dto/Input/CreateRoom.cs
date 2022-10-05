using System;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Gaming.Dto.Input;

/// <summary>
/// DTO for room creating
/// </summary>
public class CreateRoom
{
    /// <summary>
    /// Game identifier
    /// </summary>
    public Guid GameId { get; set; }

    /// <summary>
    /// Room title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Room type
    /// </summary>
    public RoomType Type { get; set; }

    /// <summary>
    /// Room access type
    /// </summary>
    public RoomAccessType AccessType { get; set; }
}