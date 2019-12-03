using DM.Services.Gaming.Dto.Output;

namespace DM.Web.Classic.Views.CreateCharacter
{
    public interface ICharacterCreateFormBuilder
    {
        CharacterCreateForm Build(GameExtended game);
    }
}