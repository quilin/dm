using System;

namespace DM.Services.Core.Dto.Enums
{
    [Flags]
    public enum AccessPolicy
    {
        NotSpecified = 0,
        DemocraticBan = 1 << 0,
        FullBan = 1 << 2,
        ChatBan = 1 << 3,
        RestrictContentEditing = 1 << 4
    }
}