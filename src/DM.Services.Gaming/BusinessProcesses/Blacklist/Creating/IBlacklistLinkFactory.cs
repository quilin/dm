using System;
using DM.Services.DataAccess.BusinessObjects.Games.Links;

namespace DM.Services.Gaming.BusinessProcesses.Blacklist.Creating;

/// <summary>
/// Factory for blacklist link DAL model
/// </summary>
internal interface IBlacklistLinkFactory
{
    /// <summary>
    /// Create DAL model
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="userId">User identifier</param>
    /// <returns></returns>
    BlackListLink Create(Guid gameId, Guid userId);
}