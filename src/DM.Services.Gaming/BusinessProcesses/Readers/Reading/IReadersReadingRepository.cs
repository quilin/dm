using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Gaming.BusinessProcesses.Readers.Reading;

/// <summary>
/// Storage for game readers
/// </summary>
internal interface IReadersReadingRepository
{
    /// <summary>
    /// Get game readers
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="userId">User identifier</param>
    /// <returns></returns>
    Task<IEnumerable<GeneralUser>> Get(Guid gameId, Guid userId);
}