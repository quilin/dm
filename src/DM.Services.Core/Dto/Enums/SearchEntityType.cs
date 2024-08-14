using System.ComponentModel;

namespace DM.Services.Core.Dto.Enums;

/// <summary>
/// Nature of indexed entity
/// </summary>
public enum SearchEntityType
{
    /// <summary>
    /// Unknown
    /// </summary>
    [Description("Неважно")]
    Unknown = 0,

    /// <summary>
    /// Forum topic
    /// </summary>
    [Description("Темы форума")]
    Topic = 1,

    /// <summary>
    /// Forum commentary
    /// </summary>
    [Description("Комментарии на форуме")]
    ForumComment = 2,

    /// <summary>
    /// Game
    /// </summary>
    [Description("Игры")]
    Game = 3,

    /// <summary>
    /// User
    /// </summary>
    [Description("Пользователи")]
    User = 4,
}