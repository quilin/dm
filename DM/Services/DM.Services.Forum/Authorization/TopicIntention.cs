namespace DM.Services.Forum.Authorization
{
    /// <summary>
    /// List of topic actions that requires authorization
    /// </summary>
    public enum TopicIntention
    {
        /// <summary>
        /// Create a comment on the topic
        /// </summary>
        CreateComment = 0,

        /// <summary>
        /// Edit topic title or description
        /// </summary>
        Edit = 1,

        /// <summary>
        /// Remove the topic with all its commentaries
        /// </summary>
        Delete = 2,

        /// <summary>
        /// Like the topic
        /// </summary>
        Like = 3
    }
}