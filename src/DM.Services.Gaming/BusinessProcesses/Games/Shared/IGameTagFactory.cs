using System;
using DM.Services.DataAccess.BusinessObjects.Games.Links;

namespace DM.Services.Gaming.BusinessProcesses.Games.Shared;

/// <summary>
/// Creates DAL model for game tag
/// </summary>
internal interface IGameTagFactory
{
    /// <summary>
    /// Create game tag DAL model
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="tagId">Tag identifier</param>
    /// <returns>Game tag link DAL model</returns>
    GameTag Create(Guid gameId, Guid tagId);
}