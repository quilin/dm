namespace DM.Services.Forum.Authorization;

/// <summary>
/// List of forum actions that required authorization
/// </summary>
public enum ForumIntention
{
    /// <summary>
    /// Create a topic on the forum
    /// </summary>
    CreateTopic = 0,

    /// <summary>
    /// Close, attach, delete and move any topic of the forum
    /// </summary>
    AdministrateTopics = 1
}