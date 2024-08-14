namespace DM.Services.Core.Dto.Enums;

/// <summary>
/// Game room access mode
/// </summary>
public enum RoomAccessType
{
    /// <summary>
    /// Anyone can view the room
    /// </summary>
    Open = 0,

    /// <summary>
    /// Only linked character players may view the room
    /// </summary>
    Secret = 1,

    /// <summary>
    /// Only GM may view the room
    /// </summary>
    Archive = 2
}