using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Gaming.BusinessProcesses.Blacklist.Reading;

/// <summary>
/// Service for reading game blacklist
/// </summary>
public interface IBlacklistReadingService
{
    /// <summary>
    /// Get list of undesired users
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<GeneralUser>> Get(Guid gameId, CancellationToken cancellationToken);
}