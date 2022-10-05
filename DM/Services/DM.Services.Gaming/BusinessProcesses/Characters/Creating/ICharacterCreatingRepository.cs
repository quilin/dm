using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Output;
using CharacterAttribute = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.CharacterAttribute;
using DbCharacter = DM.Services.DataAccess.BusinessObjects.Games.Characters.Character;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Creating;

/// <summary>
/// Character creating storage
/// </summary>
internal interface ICharacterCreatingRepository
{
    /// <summary>
    /// Save new character
    /// </summary>
    /// <param name="character">Character DAL</param>
    /// <param name="attributes"></param>
    /// <returns></returns>
    Task<Character> Create(DbCharacter character, IEnumerable<CharacterAttribute> attributes);
}