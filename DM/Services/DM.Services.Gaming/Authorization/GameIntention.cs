namespace DM.Services.Gaming.Authorization
{
    /// <summary>
    /// List of game actions that requires authorization
    /// </summary>
    public enum GameIntention
    {
        /// <summary>
        /// Create new game
        /// </summary>
        Create = 0,

        /// <summary>
        /// Read game details
        /// </summary>
        Read = 1,

        /// <summary>
        /// Edit game
        /// </summary>
        Edit = 2,

        /// <summary>
        /// Remove game
        /// </summary>
        Delete = 3
    }
}