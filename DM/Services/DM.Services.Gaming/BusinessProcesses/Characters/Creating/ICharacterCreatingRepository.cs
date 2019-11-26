using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Output;
using DbCharacter = DM.Services.DataAccess.BusinessObjects.Games.Characters.Character;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Creating
{
    /// <summary>
    /// Character creating storage
    /// </summary>
    public interface ICharacterCreatingRepository
    {
        /// <summary>
        /// Save new character
        /// </summary>
        /// <param name="character">Character DAL</param>
        /// <returns></returns>
        Task<Character> Create(DbCharacter character);
    }
}