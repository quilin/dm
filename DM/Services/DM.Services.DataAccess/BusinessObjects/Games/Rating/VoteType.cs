using System;

namespace DM.Services.DataAccess.BusinessObjects.Games.Rating
{
    [Flags]
    public enum VoteType
    {
        Unknown = 0,
        Fun = 1 << 0,
        Roleplay = 1 << 1,
        Literature = 1 << 2
    }
}