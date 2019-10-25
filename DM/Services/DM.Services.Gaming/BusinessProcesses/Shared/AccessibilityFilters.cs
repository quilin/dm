using System;
using System.Linq;
using System.Linq.Expressions;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Games;

namespace DM.Services.Gaming.BusinessProcesses.Shared
{
    /// <summary>
    /// Filters for blacklist and 
    /// </summary>
    public static class AccessibilityFilters
    {
        /// <summary>
        /// Game is accessible for user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Expression<Func<Game, bool>> GameAccessible(Guid userId) => game =>
            !game.IsRemoved &&
            game.BlackList.All(b => b.UserId != userId) &&
            (
                game.MasterId == userId ||
                game.AssistantId == userId ||
                game.NannyId == userId ||
                game.Status != GameStatus.Draft ||
                game.Status != GameStatus.RequiresModeration ||
                game.Status != GameStatus.Moderation
            );
    }
}