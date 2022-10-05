namespace DM.Services.DataAccess.BusinessObjects.Common;

/// <summary>
/// Type of possibly unread entry
/// </summary>
public enum UnreadEntryType
{
    /// <summary>
    /// Common text messages, such as posts, commentaries, private messages etc.
    /// </summary>
    Message = 0,

    /// <summary>
    /// Game characters (for GM only)
    /// </summary>
    Character = 1
}