using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Gaming.Dto;
using DM.Services.Gaming.Dto.Output;
using DtoCharacter = DM.Services.Gaming.Dto.Output.Character;

namespace DM.Web.Classic.Views.CharactersList.Character.CharacterActions
{
    public class CharacterActionsViewModelBuilder : ICharacterActionsViewModelBuilder
    {
        private readonly IIdentity identity;

        public CharacterActionsViewModelBuilder(
            IIdentityProvider identityProvider)
        {
            identity = identityProvider.Current;
        }

        public CharacterActionsViewModel Build(DtoCharacter character, Game game)
        {
            return new CharacterActionsViewModel
            {
                CharacterId = character.Id,
                HasMasterAccess = game.Participation(identity.User.UserId).HasFlag(GameParticipation.Authority)
            };
        }
    }
}