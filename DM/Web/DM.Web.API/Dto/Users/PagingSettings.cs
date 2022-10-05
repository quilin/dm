namespace DM.Web.API.Dto.Users;

/// <summary>
/// API DTO for user paging settings
/// </summary>
public class PagingSettings
{
    /// <summary>
    /// Number of posts on a game room page
    /// </summary>
    public int PostsPerPage { get; set; }

    /// <summary>
    /// Number of commentaries on a game or a topic page
    /// </summary>
    public int CommentsPerPage { get; set; }

    /// <summary>
    /// Number of detached topics on a forum page
    /// </summary>
    public int TopicsPerPage { get; set; }

    /// <summary>
    /// Number of private messages and conversations on dialogue page
    /// </summary>
    public int MessagesPerPage { get; set; }

    /// <summary>
    /// Number of other entities on page
    /// </summary>
    public int EntitiesPerPage { get; set; }
}