using DM.Web.Classic.Views.CreateCharacter;

namespace DM.Web.Classic.Dto
{
    public interface ICreateCharacterModelConverter
    {
        CreateCharacterModel Convert(CharacterCreateForm form);
    }
}