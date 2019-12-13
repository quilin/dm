namespace DM.Services.Gaming.Authorization
{
    /// <summary>
    /// List of post actions that requires authorization
    /// </summary>
    public enum PostIntention
    {
        /// <summary>
        /// Change post text
        /// </summary>
        Edit = 1,

        /// <summary>
        /// Change post character
        /// </summary>
        EditCharacter = 2,

        /// <summary>
        /// Delete post
        /// </summary>
        Delete = 3
    }
}