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

        public CharacterViewModel Build(CharacterShortInfo character, int number) => new CharacterViewModel
        {
            CharacterId = character.Id,
            User = userViewModelBuilder.Build(character.Author),
            Name = character.Name,
            Race = character.Race,
            Class = character.Class,
            Status = character.Status,
            Number = number + 1,
            PostsCount = character.PostsCount,
            LastPost = character.LastPost
        };
    }
}