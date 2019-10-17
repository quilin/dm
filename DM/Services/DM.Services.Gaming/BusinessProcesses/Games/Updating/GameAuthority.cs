using System;

namespace DM.Services.Gaming.BusinessProcesses.Games.Updating
{
    /// <summary>
    /// User authority over game
    /// </summary>
    [Flags]
    public enum GameAuthority
    {
        /// <summary>
        /// No authority
        /// </summary>
        None = 0,

        /// <summary>
        /// User is a game master or an assistant
        /// </summary>
        Owner = 1,

        /// <summary>
        /// User moderates the game
        /// </summary>
        Moderator = 1 << 1,

        /// <summary>
        /// User is allowed to moderate the game
        /// </summary>
        Nanny = 1 << 2,

        /// <summary>
        /// User has high authority to do whatever possible
        /// </summary>
        HighAuthority = 1 << 3,
    }
}