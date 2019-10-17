using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Authorization;

namespace DM.Services.Gaming.BusinessProcesses.Games.Updating
{
    /// <inheritdoc />
    public class GameIntentionConverter : IGameIntentionConverter
    {
        /// <inheritdoc />
        public (GameIntention intention, EventType eventType) Convert(GameStatus gameStatus)
        {
            switch (gameStatus)
            {
                case GameStatus.Moderation:
                    return (GameIntention.SetStatusModeration, EventType.StatusGameModeration);
                case GameStatus.Draft:
                    return (GameIntention.SetStatusDraft, EventType.StatusGameDraft);
                case GameStatus.Requirement:
                    return (GameIntention.SetStatusRequirement, EventType.StatusGameRequirement);
                case GameStatus.Active:
                    return (GameIntention.SetStatusActive, EventType.StatusGameActive);
                case GameStatus.Frozen:
                    return (GameIntention.SetStatusFrozen, EventType.StatusGameFrozen);
                case GameStatus.Finished:
                    return (GameIntention.SetStatusFinished, EventType.StatusGameFinished);
                case GameStatus.Closed:
                    return (GameIntention.SetStatusClosed, EventType.StatusGameClosed);
                default:
                    throw new GameIntentionConverterException(gameStatus);
            }
        }
    }
}