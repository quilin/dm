namespace DM.Web.API.Dto.Fora;

/// <summary>
/// API DTO model for forum
/// </summary>
public class Forum
{
    /// <summary>
    /// Forum identifier
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Total count of topics with unread commentaries within
    /// </summary>
    public int UnreadTopicsCount { get; set; }
}