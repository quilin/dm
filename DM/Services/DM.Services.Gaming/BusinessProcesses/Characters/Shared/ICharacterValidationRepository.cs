using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Shared;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Shared
{
    /// <summary>
    /// Storage for character attributes validation
    /// </summary>
    public interface ICharacterValidationRepository
    {
        /// <summary>
        /// Game requires attributes
        /// </summary>
        /// <param name="createCharacter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> GameRequiresAttributes(CreateCharacter createCharacter, CancellationToken cancellationToken);

        /// <summary>
        /// Get schema for game
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        Task<AttributeSchema> GetSchema(Guid gameId);
    }
}