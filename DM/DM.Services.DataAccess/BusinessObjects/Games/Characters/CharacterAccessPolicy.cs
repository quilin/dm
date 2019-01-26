using System;

namespace DM.Services.DataAccess.BusinessObjects.Games.Characters
{
    [Flags]
    public enum CharacterAccessPolicy
    {
        NoAccess = 0,
        EditAllowed = 1 << 0,
        PostEditAllowed = 1 << 1
    }
}