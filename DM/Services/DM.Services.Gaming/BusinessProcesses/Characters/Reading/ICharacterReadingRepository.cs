using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Reading;

/// <summary>
/// Storage for character reading
/// </summary>
internal interface ICharacterReadingRepository
{
    /// <summary>
    /// Get all game characters
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <returns></returns>
    Task<IEnumerable<Character>> GetCharacters(Guid gameId);

    /// <summary>
    /// Get single character
    /// </summary>
    /// <param name="characterId">Character identifier</param>
    /// <returns></returns>
    Task<Character> FindCharacter(Guid characterId);
}