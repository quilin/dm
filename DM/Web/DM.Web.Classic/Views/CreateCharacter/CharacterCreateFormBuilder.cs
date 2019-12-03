using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Parsing;
using DM.Services.Gaming.Dto;
using DM.Services.Gaming.Dto.Output;

namespace DM.Web.Classic.Views.CreateCharacter
{
    public class CharacterCreateFormBuilder : ICharacterCreateFormBuilder
    {
        private readonly IBbParserProvider bbParserProvider;
        private readonly IIdentity identity;

        public CharacterCreateFormBuilder(
            IBbParserProvider bbParserProvider,
            IIdentityProvider identityProvider)
        {
            this.bbParserProvider = bbParserProvider;
            identity = identityProvider.Current;
        }

        public CharacterCreateForm Build(GameExtended game)
        {
            var hasMasterAccess = game.Participation(identity.User.UserId).HasFlag(GameParticipation.Authority);
            return new CharacterCreateForm
            {
                GameId = game.Id,

                TemperHidden = game.HideTemper,
                StoryHidden = game.HideStory,
                SkillsHidden = game.HideSkills,
                InventoryHidden = game.HideInventory,

                IsNpc = hasMasterAccess,
                HasMasterAccess = hasMasterAccess,
                DisplayAlignment = !game.DisableAlignment,

                Parser = bbParserProvider.CurrentCommon
            };
        }
    }
}