using System;
using System.Collections.Generic;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.Gaming.Dto.Input;
using DbGame = DM.Services.DataAccess.BusinessObjects.Games.Game;
using DbRoom = DM.Services.DataAccess.BusinessObjects.Games.Posts.Room;

namespace DM.Services.Gaming.BusinessProcesses.Games.Creating
{
    /// <summary>
    /// Factory for game DAL model
    /// </summary>
    public interface IGameFactory
    {
        /// <summary>
        /// Create game and its initial room out of DTO model
        /// </summary>
        /// <param name="createGame">DTO model</param>
        /// <param name="masterId">User identifier</param>
        /// <param name="assistantId">Assistant user identifier</param>
        /// <param name="initialStatus">Initial game status</param>
        /// <returns>Pair of game DAL and its initial room DAL</returns>
        (DbGame game, DbRoom room, IEnumerable<GameTag> tags) Create(CreateGame createGame, Guid masterId,
            Guid? assistantId, GameStatus initialStatus);
    }
}