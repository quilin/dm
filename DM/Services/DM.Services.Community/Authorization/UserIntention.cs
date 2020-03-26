namespace DM.Services.Community.Authorization
{
    /// <summary>
    /// List of user actions that require authorization
    /// </summary>
    public enum UserIntention
    {
        /// <summary>
        /// Edit user details
        /// </summary>
        Edit = 1,

        /// <summary>
        /// Participate in dialogues with user
        /// </summary>
        WriteMessage = 2
    }
}