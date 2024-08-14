using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Authorization;

namespace DM.Services.Gaming.BusinessProcesses.Games.Updating;

/// <summary>
/// Converter for game status into associated intention
/// </summary>
internal interface IGameIntentionConverter
{
    /// <summary>
    /// Converts game status to associated intention
    /// </summary>
    /// <param name="gameStatus">Game status</param>
    /// <returns>Authorized intention and invoked event type</returns>
    (GameIntention intention, EventType eventType) Convert(GameStatus gameStatus);
}