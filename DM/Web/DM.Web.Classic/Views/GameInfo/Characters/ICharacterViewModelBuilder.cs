using DM.Services.Gaming.Dto.Output;

namespace DM.Web.Classic.Views.GameInfo.Characters
{
    public interface ICharacterViewModelBuilder
    {
        CharacterViewModel Build(CharacterShortInfo character, int number);
    }
}