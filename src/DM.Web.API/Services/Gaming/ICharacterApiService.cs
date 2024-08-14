using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games;

namespace DM.Web.API.Services.Gaming;

/// <summary>
/// API service for game character resources
/// </summary>
public interface ICharacterApiService
{
    /// <summary>
    /// Get list of game characters
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <returns></returns>
    Task<ListEnvelope<Character>> GetAll(Guid gameId);

    /// <summary>
    /// Get single character
    /// </summary>
    /// <param name="characterId">Character identifier</param>
    /// <returns></returns>
    Task<Envelope<Character>> Get(Guid characterId);

    /// <summary>
    /// Create new character
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="character">Character API model</param>
    /// <returns></returns>
    Task<Envelope<Character>> Create(Guid gameId, Character character);

    /// <summary>
    /// Update existing character
    /// </summary>
    /// <param name="characterId">Character identifier</param>
    /// <param name="character">Character API model</param>
    /// <returns></returns>
    Task<Envelope<Character>> Update(Guid characterId, Character character);

    /// <summary>
    /// Delete existing character
    /// </summary>
    /// <param name="characterId">Character identifier</param>
    /// <returns></returns>
    Task Delete(Guid characterId);

    /// <summary>
    /// Mark all characters as read
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <returns></returns>
    Task MarkAsRead(Guid gameId);
}