using System;

namespace DM.Services.Core.Dto.Enums
{
    [Flags]
    public enum ForumAccessPolicy
    {
        NoOne = 0,
        Administrator = 1 << 0,
        SeniorModerator = 1 << 1,
        RegularModerator = 1 << 2,
        NurseModerator = 1 << 3,
        ForumModerator = 1 << 4,
        Player = 1 << 5,
        Guest = 1 << 6
    }
}