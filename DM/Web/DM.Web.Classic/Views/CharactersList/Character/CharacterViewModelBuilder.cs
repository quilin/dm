using System;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Core.Parsing;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Characters.Reading;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.Dto.Output;
using DM.Web.Classic.Views.CharactersList.Character.CharacterActions;
using DM.Web.Classic.Views.Shared.User;
using DtoCharacter = DM.Services.Gaming.Dto.Output.Character;

namespace DM.Web.Classic.Views.CharactersList.Character
{
    public class CharacterViewModelBuilder : ICharacterViewModelBuilder
    {
        private readonly IGameReadingService gameService;
        private readonly ICharacterReadingService characterService;
        private readonly ICharacterActionsViewModelBuilder characterActionsViewModelBuilder;
        private readonly IBbParserProvider bbParserProvider;
        private readonly IIntentionManager intentionManager;
        private readonly IUserViewModelBuilder userViewModelBuilder;

        public CharacterViewModelBuilder(
            IGameReadingService gameService,
            ICharacterReadingService characterService,
            ICharacterActionsViewModelBuilder characterActionsViewModelBuilder,
            IBbParserProvider bbParserProvider,
            IIntentionManager intentionManager,
            IUserViewModelBuilder userViewModelBuilder)
        {
            this.gameService = gameService;
            this.characterService = characterService;
            this.characterActionsViewModelBuilder = characterActionsViewModelBuilder;
            this.bbParserProvider = bbParserProvider;
            this.intentionManager = intentionManager;
            this.userViewModelBuilder = userViewModelBuilder;
        }

        public async Task<CharacterViewModel> Build(DtoCharacter character, GameExtended game)
        {
            var bbParser = bbParserProvider.CurrentCommon;
            return new CharacterViewModel
            {
                CharacterId = character.Id,
                Author = userViewModelBuilder.Build(character.Author),
                Status = character.Status,
                Name = character.Name,
                Race = character.Race,
                Class = character.Class,
                Alignment = character.Alignment,
                Attributes = character.Attributes,
                PictureUrl = character.PictureUrl,
                Appearance = bbParser.Parse(character.Appearance).ToHtml(),
                Temper = bbParser.Parse(character.Temper).ToHtml(),
                DisplayTemper = intentionManager.IsAllowed(CharacterIntention.ViewTemper, (character, game)),
                TemperHidden = game.HideTemper,
                Skills = bbParser.Parse(character.Skills).ToHtml(),
                DisplaySkills = intentionManager.IsAllowed(CharacterIntention.ViewSkills, (character, game)),
                SkillsHidden = game.HideSkills,
                Inventory = bbParser.Parse(character.Inventory).ToHtml(),
                DisplayInventory = intentionManager.IsAllowed(CharacterIntention.ViewInventory, (character, game)),
                InventoryHidden = game.HideInventory,
                Story = bbParser.Parse(character.Story).ToHtml(),
                DisplayStory = intentionManager.IsAllowed(CharacterIntention.ViewStory, (character, game)),
                StoryHidden = game.HideStory,
                DisplayAlignment = !game.DisableAlignment,
                CharacterActions = characterActionsViewModelBuilder.Build(character, game)
            };
        }

        public async Task<CharacterViewModel> Build(Guid characterId)
        {
            var character = await characterService.GetCharacter(characterId);
            var game = await gameService.GetGameDetails(character.GameId);
            return await Build(character, game);
        }
    }
}