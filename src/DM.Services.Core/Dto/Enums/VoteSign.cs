namespace DM.Services.Core.Dto.Enums;

/// <summary>
/// Post rating sign
/// </summary>
public enum VoteSign
{
    /// <summary>
    /// User did not like the post
    /// </summary>
    Negative = -1,

    /// <summary>
    /// User wanted to comment the post without changing author's rating
    /// </summary>
    Neutral = 0,

    /// <summary>
    /// User liked the post
    /// </summary>
    Positive = 1
}