namespace DM.Services.Core.Dto.Enums
{
    /// <summary>
    /// Game status
    /// </summary>
    public enum GameStatus
    {
        /// <summary>
        /// Game is closed inconclusively
        /// </summary>
        Closed = 0,

        /// <summary>
        /// Game is finished successfully
        /// </summary>
        Finished = 1,

        /// <summary>
        /// Game is frozen due to inactivity
        /// </summary>
        Frozen = 2,

        /// <summary>
        /// Game requires players and characters
        /// </summary>
        Requirement = 3,

        /// <summary>
        /// Game is not yet published
        /// </summary>
        Draft = 4,

        /// <summary>
        /// Game has started
        /// </summary>
        Active = 5,

        /// <summary>
        /// GM is newbie so the game requires moderation, no moderator is assigned yet
        /// </summary>
        RequiresModeration = 6,

        /// <summary>
        /// Game is on moderation and awaits moderator
        /// </summary>
        Moderation = 7
    }
}