using System;

namespace DM.Services.Core.Dto.Enums
{
    [Flags]
    public enum CharacterAccessPolicy
    {
        NoAccess = 0,
        EditAllowed = 1 << 0,
        PostEditAllowed = 1 << 1
    }
}