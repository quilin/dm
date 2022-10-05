using System;

namespace DM.Services.Core.Dto.Enums;

/// <summary>
/// Character access policy for GM
/// </summary>
[Flags]
public enum CharacterAccessPolicy
{
    /// <summary>
    /// Only the author has access to character
    /// </summary>
    NoAccess = 0,

    /// <summary>
    /// GM may edit character information
    /// </summary>
    EditAllowed = 1,

    /// <summary>
    /// GM may edit character post texts
    /// </summary>
    PostEditAllowed = 1 << 1
}