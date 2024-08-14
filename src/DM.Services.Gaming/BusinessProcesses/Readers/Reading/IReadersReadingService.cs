using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Gaming.BusinessProcesses.Readers.Reading;

/// <summary>
/// Service for reading game readers
/// </summary>
public interface IReadersReadingService
{
    /// <summary>
    /// Get all game readers
    /// </summary>
    /// <param name="gameId">Game identifiers</param>
    /// <returns></returns>
    Task<IEnumerable<GeneralUser>> Get(Guid gameId);
}