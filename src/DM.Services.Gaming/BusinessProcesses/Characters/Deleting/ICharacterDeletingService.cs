using System;
using System.Threading;
using System.Threading.Tasks;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Deleting;

/// <summary>
/// Service for character deleting
/// </summary>
public interface ICharacterDeletingService
{
    /// <summary>
    /// Delete existing character
    /// </summary>
    /// <param name="characterId">Character identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Delete(Guid characterId, CancellationToken cancellationToken);
}