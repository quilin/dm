using System.Linq;
using DM.Web.Classic.Views.CreateCharacter;

namespace DM.Web.Classic.Dto
{
    public class CreateCharacterModelConverter : ICreateCharacterModelConverter
    {
        public CreateCharacterModel Convert(CharacterCreateForm form)
        {
            return new CreateCharacterModel
            {
                ModuleId = form.ModuleId,
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