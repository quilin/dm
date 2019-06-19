using System.Linq;
using DM.Web.Classic.Views.EditCharacter;

namespace DM.Web.Classic.Dto
{
    public class UpdateCharacterModelConverter : IUpdateCharacterModelConverter
    {
        public UpdateCharacterModel Convert(CharacterEditForm form)
        {
            return new UpdateCharacterModel
            {
                CharacterId = form.CharacterId,
                Name = form.Name,
                Race = form.Race,
                ClassName = form.Class,
                Alignment = form.Alignment,
                Appearance = form.Appearance,
                Temper = form.Temper,
                Skills = form.Skills,
                Inventory = form.Inventory,
                Story = form.Story,
                Attributes = form.CharacterAttributes?.ToDictionary(a => a.AttributeSpecificationId, a => a.Value),
                IsNpc = form.IsNpc,
                MasterEditAllowed = form.MasterEditAllowed,
                MasterPostsEditAllowed = form.MasterPostsEditAllowed,
                PictureUploadRootId = form.PictureUploadRootId
            };
        }
    }
}