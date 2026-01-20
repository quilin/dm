using System;
using System.Threading;
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
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ListEnvelope<Character>> GetAll(Guid gameId, CancellationToken cancellationToken);

    /// <summary>
    /// Get single character
    /// </summary>
    /// <param name="characterId">Character identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<Character>> Get(Guid characterId, CancellationToken cancellationToken);

    /// <summary>
    /// Create new character
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="character">Character API model</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<Character>> Create(Guid gameId, Character character, CancellationToken cancellationToken);

    /// <summary>
    /// Update existing character
    /// </summary>
    /// <param name="characterId">Character identifier</param>
    /// <param name="character">Character API model</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<Character>> Update(Guid characterId, Character character, CancellationToken cancellationToken);

    /// <summary>
    /// Delete existing character
    /// </summary>
    /// <param name="characterId">Character identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Delete(Guid characterId, CancellationToken cancellationToken);

    /// <summary>
    /// Mark all characters as read
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task MarkAsRead(Guid gameId, CancellationToken cancellationToken);
}