using System;

namespace DM.Services.Core.Dto.Enums;

/// <summary>
/// User role
/// </summary>
[Flags]
public enum UserRole
{
    /// <summary>
    /// Unauthenticated user
    /// </summary>
    Guest = 0,

    /// <summary>
    /// Authenticated user with no privileges
    /// </summary>
    Player = 1,

    /// <summary>
    /// Developer / Owner
    /// </summary>
    Administrator = 1 << 1,

    /// <summary>
    /// Helps newbies, can premoderate games
    /// </summary>
    NannyModerator = 1 << 2,

    /// <summary>
    /// Moderates a certain forum or the chat, can give warnings, close inactive games
    /// </summary>
    RegularModerator = 1 << 3,

    /// <summary>
    /// Moderates the site, can give warnings, bans and deal with complaints on regular moderators
    /// </summary>
    SeniorModerator = 1 << 4
}