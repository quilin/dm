using DM.Services.Gaming.Dto.Output;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.GameInfo.Characters
{
    public class CharacterViewModelBuilder : ICharacterViewModelBuilder
    {
        private readonly IUserViewModelBuilder userViewModelBuilder;

        public CharacterViewModelBuilder(
            IUserViewModelBuilder userViewModelBuilder)
        {
            this.userViewModelBuilder = userViewModelBuilder;
        }

        public CharacterViewModel Build(Character character, int number)
        {
            return new CharacterViewModel
            {
                CharacterId = character.Id,
                User = userViewModelBuilder.Build(character.Author),
                Name = character.Name,
                Race = character.Race,
                Class = character.Class,
                Status = character.Status,
                Number = number + 1
            };
        }
    }
}