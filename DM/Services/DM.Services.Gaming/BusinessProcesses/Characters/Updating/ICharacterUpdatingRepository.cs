using System;
using System.Threading.Tasks;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Dto.Internal;
using DM.Services.Gaming.Dto.Output;
using DbCharacter = DM.Services.DataAccess.BusinessObjects.Games.Characters.Character;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Updating
{
    /// <summary>
    /// Character updating storage
    /// </summary>
    public interface ICharacterUpdatingRepository
    {
        /// <summary>
        /// Get character for updating
        /// </summary>
        /// <param name="characterId">Character identifier</param>
        /// <returns></returns>
        Task<CharacterToUpdate> Get(Guid characterId);

        /// <summary>
        /// Update character
        /// </summary>
        /// <param name="updateCharacter">Update builder</param>
        /// <returns></returns>
        Task<Character> Update(IUpdateBuilder<DbCharacter> updateCharacter);
    }
}