using System;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Gaming.Dto.Input;

/// <summary>
/// DTO for room updating
/// </summary>
public class UpdateRoom
{
    /// <summary>
    /// Room identifier
    /// </summary>
    public Guid RoomId { get; set; }

    /// <summary>
    /// Room title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Room type
    /// </summary>
    public RoomType? Type { get; set; }

    /// <summary>
    /// Room access type
    /// </summary>
    public RoomAccessType? AccessType { get; set; }

    /// <summary>
    /// Previous room identifier
    /// </summary>
    public Optional<Guid> PreviousRoomId { get; set; }
}