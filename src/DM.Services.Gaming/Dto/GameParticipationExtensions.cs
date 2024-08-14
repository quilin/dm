using System;
using System.Linq;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.Dto;

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
    /// <returns>Game participation type</returns>
    public static GameParticipation Participation(this Game game, Guid userId)
    {
        var participation = GameParticipation.None;
        if (game.Master.UserId == userId)
        {
            participation |= GameParticipation.Owner | GameParticipation.Authority;
        }

        if (game.Assistant?.UserId == userId)
        {
            participation |= GameParticipation.Authority;
        }

        if (game.PendingAssistant?.UserId == userId)
        {
            participation |= GameParticipation.PendingAssistant;
        }

        if (game.Nanny?.UserId == userId)
        {
            participation |= GameParticipation.Moderator;
        }

        if (game.ActiveCharacterUserIds.Contains(userId))
        {
            participation |= GameParticipation.Player;
        }

        if (game.ReaderUserIds.Contains(userId))
        {
            participation |= GameParticipation.Reader;
        }

        return participation;
    }
}