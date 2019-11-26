using System;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Dto.Input;
using DbCharacter = DM.Services.DataAccess.BusinessObjects.Games.Characters.Character;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Creating
{
    /// <summary>
    /// Factory for character DAL model
    /// </summary>
    public interface ICharacterFactory
    {
        /// <summary>
        /// Create character from DTO model
        /// </summary>
        /// <param name="createCharacter">DTO model</param>
        /// <param name="userId">User identifier</param>
        /// <param name="initialStatus">Initial character status</param>
        /// <returns>Character DAL</returns>
        DbCharacter Create(CreateCharacter createCharacter, Guid userId, CharacterStatus initialStatus);
    }
}