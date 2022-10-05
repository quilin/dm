namespace DM.Web.API.BbRendering;

/// <summary>
/// BB text parse mode
/// </summary>
public enum BbParseMode
{
    /// <summary>
    /// General text parse mode
    /// </summary>
    Common = 0,

    /// <summary>
    /// General information parse mode
    /// </summary>
    Info = 1,

    /// <summary>
    /// Game post parse mode
    /// </summary>
    Post = 3,

    /// <summary>
    /// Chat message parse mode
    /// </summary>
    Chat = 4
}