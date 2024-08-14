namespace DM.Services.Forum.Dto.Input;

/// <summary>
/// DTO model for creating new topic
/// </summary>
public class CreateTopic
{
    /// <summary>
    /// Parent forum title
    /// </summary>
    public string ForumTitle { get; set; }

    /// <summary>
    /// New topic title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// New topic description text
    /// </summary>
    public string Text { get; set; }
}