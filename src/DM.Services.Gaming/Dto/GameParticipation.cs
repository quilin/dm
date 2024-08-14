using System;

namespace DM.Services.Gaming.Dto;

/// <summary>
/// Types of game participation
/// </summary>
[Flags]
public enum GameParticipation
{
    /// <summary>
    /// User doesn't participate in game
    /// </summary>
    None = 0,

    /// <summary>
    /// User is reading this game
    /// </summary>
    Reader = 1,

    /// <summary>
    /// User has an active character in game
    /// </summary>
    Player = 1 << 1,

    /// <summary>
    /// User is game nanny
    /// </summary>
    Moderator = 1 << 2,

    /// <summary>
    /// User is a pending assistant
    /// </summary>
    PendingAssistant = 1 << 3,

    /// <summary>
    /// User is either master or active assistant
    /// </summary>
    Authority = 1 << 4,

    /// <summary>
    /// User is creator and master of the game
    /// </summary>
    Owner = 1 << 5
}