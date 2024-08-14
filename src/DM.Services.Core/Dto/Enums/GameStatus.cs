using System.ComponentModel;

namespace DM.Services.Core.Dto.Enums;

/// <summary>
/// Game status
/// </summary>
public enum GameStatus
{
    /// <summary>
    /// Game is closed inconclusively
    /// </summary>
    [Description("Закрыта")]
    Closed = 0,

    /// <summary>
    /// Game is finished successfully
    /// </summary>
    [Description("Завершена")]
    Finished = 1,

    /// <summary>
    /// Game is frozen due to inactivity
    /// </summary>
    [Description("Заморожена")]
    Frozen = 2,

    /// <summary>
    /// Game requires players and characters
    /// </summary>
    [Description("Набор игроков")]
    Requirement = 3,

    /// <summary>
    /// Game is not yet published
    /// </summary>
    [Description("Черновик")]
    Draft = 4,

    /// <summary>
    /// Game has started
    /// </summary>
    [Description("Идет игра")]
    Active = 5,

    /// <summary>
    /// GM is newbie so the game requires moderation, no moderator is assigned yet
    /// </summary>
    [Description("Ожидает модерации")]
    RequiresModeration = 6,

    /// <summary>
    /// Game is on moderation and awaits moderator
    /// </summary>
    [Description("На модерации")]
    Moderation = 7
}