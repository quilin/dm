namespace DM.Services.Core.Dto.Enums;

/// <summary>
/// Access policy to room
/// </summary>
public enum RoomAccessPolicy
{
    /// <summary>
    /// No access to room
    /// </summary>
    NoAccess = 0,

    /// <summary>
    /// Read only access
    /// </summary>
    ReadOnly = 1,

    /// <summary>
    /// Can write posts access
    /// </summary>
    Full = 2
}