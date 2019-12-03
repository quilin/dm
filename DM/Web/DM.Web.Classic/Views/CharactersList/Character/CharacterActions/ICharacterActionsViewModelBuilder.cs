using DM.Services.Gaming.Dto.Output;
using DtoCharacter = DM.Services.Gaming.Dto.Output.Character;

namespace DM.Web.Classic.Views.CharactersList.Character.CharacterActions
{
    public interface ICharacterActionsViewModelBuilder
    {
        CharacterActionsViewModel Build(DtoCharacter character, Game game);
    }
}