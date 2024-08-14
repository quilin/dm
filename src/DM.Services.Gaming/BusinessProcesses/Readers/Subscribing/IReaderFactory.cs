using System;
using DM.Services.DataAccess.BusinessObjects.Games.Links;

namespace DM.Services.Gaming.BusinessProcesses.Readers.Subscribing;

/// <summary>
/// Factory for a reader DAL
/// </summary>
internal interface IReaderFactory
{
    /// <summary>
    /// Create reader DAL
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <param name="gameId">Game identifier</param>
    /// <returns></returns>
    Reader Create(Guid userId, Guid gameId);
}