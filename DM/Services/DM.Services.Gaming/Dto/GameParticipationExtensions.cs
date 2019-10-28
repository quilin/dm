using System;
using System.Linq;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.Dto
{
    /// <summary>
    /// Extensions for participation resolving
    /// </summary>
    public static class GameParticipationExtensions
    {
        /// <summary>
        /// Tells if user participates in a game
        /// </summary>
        /// <param name="game">Mapped game</param>
        /// <param name="userId">User identifier</param>
        /// <returns></returns>
        public static bool UserParticipates(this Game game, Guid userId)
        {
            return game.Master.UserId == userId || game.Assistant?.UserId == userId ||
                game.PendingAssistant?.UserId == userId ||
                game.Nanny?.UserId == userId ||
                game.ActiveCharacterUserIds.Any(authorId => authorId == userId);
        }
    }
}