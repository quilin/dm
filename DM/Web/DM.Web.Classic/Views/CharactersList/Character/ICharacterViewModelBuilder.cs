using System;
using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Output;
using DtoCharacter = DM.Services.Gaming.Dto.Output.Character;

namespace DM.Web.Classic.Views.CharactersList.Character
{
    public interface ICharacterViewModelBuilder
    {
        CharacterViewModel Build(DtoCharacter character, GameExtended game);
        Task<CharacterViewModel> Build(Guid characterId);
    }
}