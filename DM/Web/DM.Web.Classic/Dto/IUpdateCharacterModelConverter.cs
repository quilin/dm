using DM.Web.Classic.Views.EditCharacter;

namespace DM.Web.Classic.Dto
{
    public interface IUpdateCharacterModelConverter
    {
        UpdateCharacterModel Convert(CharacterEditForm form);
    }
}