using System;

namespace DM.Services.Core.Dto.Enums;

/// <summary>
/// Individual user composite access policy
/// </summary>
[Flags]
public enum AccessPolicy
{
    /// <summary>
    /// No restrictions
    /// </summary>
    NotSpecified = 0,

    /// <summary>
    /// Democratic ban restrictions
    /// </summary>
    DemocraticBan = 1 << 0,

    /// <summary>
    /// Full ban restrictions
    /// </summary>
    FullBan = 1 << 2,

    /// <summary>
    /// Chat ban restrictions
    /// </summary>
    ChatBan = 1 << 3,

    /// <summary>
    /// Content editing restrictions
    /// </summary>
    RestrictContentEditing = 1 << 4
}