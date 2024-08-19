using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Reading;

/// <summary>
/// Service for character reading
/// </summary>
public interface ICharacterReadingService
{
    /// <summary>
    /// Get all game characters
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<Character>> GetCharacters(Guid gameId, CancellationToken cancellationToken);

    /// <summary>
    /// Get single character
    /// </summary>
    /// <param name="characterId">Character identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Character> GetCharacter(Guid characterId, CancellationToken cancellationToken);

    /// <summary>
    /// Mark all characters in game as read
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task MarkAsRead(Guid gameId, CancellationToken cancellationToken);
}