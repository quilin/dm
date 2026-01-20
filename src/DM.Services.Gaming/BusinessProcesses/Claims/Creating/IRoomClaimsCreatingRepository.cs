using System;
using System.Threading;
using System.Threading.Tasks;
using RoomClaim = DM.Services.DataAccess.BusinessObjects.Games.Links.RoomClaim;

namespace DM.Services.Gaming.BusinessProcesses.Claims.Creating;

/// <summary>
/// Storage for room claims creating
/// </summary>
internal interface IRoomClaimsCreatingRepository
{
    /// <summary>
    /// Save room claim
    /// </summary>
    /// <param name="claim">DAL model</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Dto.Output.RoomClaim> Create(RoomClaim claim, CancellationToken cancellationToken);

    /// <summary>
    /// Find reader in game
    /// </summary>
    /// <param name="gameId">Game identifier</param>
    /// <param name="readerLogin">Reader login</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Guid?> FindReaderId(Guid gameId, string readerLogin, CancellationToken cancellationToken);

    /// <summary>
    /// Get game identifier for character
    /// </summary>
    /// <param name="characterId">Character identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Guid?> FindCharacterGameId(Guid characterId, CancellationToken cancellationToken);
}