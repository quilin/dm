using System;
using System.Linq;
using System.Linq.Expressions;
using DM.Services.Core.Dto.Enums;
using Game = DM.Services.DataAccess.BusinessObjects.Games.Game;
using Room = DM.Services.DataAccess.BusinessObjects.Games.Posts.Room;

namespace DM.Services.Gaming.BusinessProcesses.Shared;

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
    public static Expression<Func<Game, bool>> GameAvailable(Guid userId) => game =>
        !game.IsRemoved &&
        !(
            game.BlackList != null &&
            game.BlackList.Any(b => b.UserId == userId)
        ) &&
        (
            game.MasterId == userId ||
            game.AssistantId == userId ||
            game.NannyId == userId ||
            game.Status != GameStatus.Draft ||
            game.Status != GameStatus.RequiresModeration ||
            game.Status != GameStatus.Moderation
        );

    /// <summary>
    /// Room is accessible for user
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public static Expression<Func<Room, bool>> RoomAvailable(Guid userId) => room =>
        !room.IsRemoved &&
        !room.Game.IsRemoved &&
        !(
            room.Game.BlackList != null &&
            room.Game.BlackList.Any(b => b.UserId == userId)
        ) &&
        (
            room.Game.MasterId == userId ||
            room.Game.AssistantId == userId ||
            room.Game.NannyId == userId ||
            room.Game.Status != GameStatus.Draft ||
            room.Game.Status != GameStatus.RequiresModeration ||
            room.Game.Status != GameStatus.Moderation
        ) &&
        (
            room.AccessType == RoomAccessType.Open ||
            room.AccessType == RoomAccessType.Secret &&
            (
                room.Game.MasterId == userId ||
                room.Game.AssistantId == userId
            ) ||
            room.RoomClaims.Any(l => l.Character.UserId == userId || l.Reader.UserId == userId)
        );
}