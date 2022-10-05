using System;

namespace DM.Services.Gaming.Dto.Input;

/// <summary>
/// DTO for blacklist link creating
/// </summary>
public class OperateBlacklistLink
{
    /// <summary>
    /// Game identifier
    /// </summary>
    public Guid GameId { get; set; }

    /// <summary>
    /// User login
    /// </summary>
    public string Login { get; set; }
}