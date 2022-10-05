using System;
using System.Collections.Generic;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Dto.Input;
using DbCharacter = DM.Services.DataAccess.BusinessObjects.Games.Characters.Character;
using DbAttribute = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.CharacterAttribute;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Creating;

/// <summary>
/// Factory for character DAL model
/// </summary>
internal interface ICharacterFactory
{
    /// <summary>
    /// Create character from DTO model
    /// </summary>
    /// <param name="createCharacter">DTO model</param>
    /// <param name="userId">User identifier</param>
    /// <param name="initialStatus">Initial character status</param>
    /// <returns>Character DAL</returns>
    (DbCharacter, IEnumerable<DbAttribute>) Create(CreateCharacter createCharacter, Guid userId,
        CharacterStatus initialStatus);
}