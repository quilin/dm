using System;
using System.Collections.Generic;
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
    /// <returns></returns>
    Task<IEnumerable<GeneralUser>> Get(Guid gameId);
}