using System;

namespace DM.Services.Core.Dto.Enums;

/// <summary>
/// Role restrictions to perform some action with fora
/// </summary>
[Flags]
public enum ForumAccessPolicy
{
    /// <summary>
    /// No one is allowed
    /// </summary>
    NoOne = 0,

    /// <summary>
    /// Administrators allowed
    /// </summary>
    Administrator = 1 << 0,

    /// <summary>
    /// Senior moderators allowed
    /// </summary>
    SeniorModerator = 1 << 1,

    /// <summary>
    /// Regular moderators allowed
    /// </summary>
    RegularModerator = 1 << 2,

    /// <summary>
    /// Nanny moderators allowed
    /// </summary>
    NannyModerator = 1 << 3,

    /// <summary>
    /// Forum moderators allowed
    /// </summary>
    ForumModerator = 1 << 4,

    /// <summary>
    /// Any authenticated user allowed
    /// </summary>
    Player = 1 << 5,

    /// <summary>
    /// Anyone allowed
    /// </summary>
    Guest = 1 << 6
}