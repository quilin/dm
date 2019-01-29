using System;

namespace DM.Services.DataAccess.BusinessObjects.Fora
{
    [Flags]
    public enum ForumAccessPolicy
    {
        NoOne = 0,
        Administrators = 1 << 0,
        SeniorModerators = 1 << 1,
        RegularModerators = 1 << 2,
        NannyModerators = 1 << 3,
        ForumModerators = 1 << 4,
        Players = 1 << 5,
        Everyone = 1 << 6
    }
}