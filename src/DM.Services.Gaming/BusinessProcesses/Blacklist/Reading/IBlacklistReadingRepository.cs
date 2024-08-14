using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Gaming.BusinessProcesses.Blacklist.Reading;

/// <summary>
/// Storage for game blacklist
/// </summary>
internal interface IBlacklistReadingRepository
{
    /// <summary>
    /// Get blacklisted users
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <returns></returns>
    Task<IEnumerable<GeneralUser>> Get(Guid gameId);
}