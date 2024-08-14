using BBCodeParser;

namespace DM.Services.Core.Parsing;

/// <summary>
/// BBCode parsers provider
/// </summary>
public interface IBbParserProvider
{
    /// <summary>
    /// General parser for every default case
    /// </summary>
    IBbParser CurrentCommon { get; }

    /// <summary>
    /// Parser for game information
    /// </summary>
    IBbParser CurrentInfo { get; }

    /// <summary>
    /// Parser for private conversation messages
    /// </summary>
    IBbParser CurrentConversationMessage { get; }

    /// <summary>
    /// Parser for game posts
    /// </summary>
    IBbParser CurrentPost { get; }

    /// <summary>
    /// Parser for game posts (NSFW)
    /// </summary>
    IBbParser CurrentSafePost { get; }

    /// <summary>
    /// Parser for rating votes (NSFW)
    /// </summary>
    IBbParser CurrentSafeRating { get; }

    /// <summary>
    /// Parser for chat messages
    /// </summary>
    IBbParser CurrentGeneralChat { get; }
}