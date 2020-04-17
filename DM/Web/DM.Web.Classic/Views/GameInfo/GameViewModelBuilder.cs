using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Parsing;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Web.Classic.Views.GameInfo.Characters;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.GameInfo
{
    public class GameViewModelBuilder : IGameViewModelBuilder
    {
        private readonly IBbParserProvider bbParserProvider;
        private readonly IUserViewModelBuilder userViewModelBuilder;
        private readonly ICharacterViewModelBuilder characterViewModelBuilder;
        private readonly IGameReadingService gameReadingService;

        public GameViewModelBuilder(
            IBbParserProvider bbParserProvider,
            IUserViewModelBuilder userViewModelBuilder,
            ICharacterViewModelBuilder characterViewModelBuilder,
            IGameReadingService gameReadingService)
        {
            this.bbParserProvider = bbParserProvider;
            this.userViewModelBuilder = userViewModelBuilder;
            this.characterViewModelBuilder = characterViewModelBuilder;
            this.gameReadingService = gameReadingService;
        }

        public async Task<GameViewModel> Build(Guid gameId)
        {
            var game = await gameReadingService.GetGameDetails(gameId);

            var charactersViewModels = game.Characters
                .Where(c => !c.IsNpc)
                .Select((c, i) => characterViewModelBuilder.Build(c, i))
                .ToArray();
            var npcsViewModels = game.Characters
                .Where(c => c.IsNpc)
                .Select((c, i) => characterViewModelBuilder.Build(c, i))
                .ToArray();

            var readers = game.Readers
                .Select(userViewModelBuilder.Build)
                .ToArray();

            var assistantConfirmed = game.Assistant != null;
            var assistant = game.Assistant ?? game.PendingAssistant;

            return new GameViewModel
            {
                GameId = game.Id,
                Status = game.Status,
                CreateDate = game.CreateDate,
                ReleaseDate = game.ReleaseDate,

                Title = game.Title,
                SystemName = game.SystemName,
                SettingName = game.SettingName,
                Info = bbParserProvider.CurrentInfo.Parse(game.Info).ToHtml(),

                Master = userViewModelBuilder.Build(game.Master),
                Assistant = assistant == null ? null : userViewModelBuilder.Build(assistant),
                AssistantConfirmed = assistantConfirmed,

                Readers = readers,
                Tags = game.Tags,

                ActiveCharactersCount = game.Characters.Count(c => !c.IsNpc && c.Status == CharacterStatus.Active),
                RegisteredCharactersCount = game.Characters.Count(c => !c.IsNpc && c.Status == CharacterStatus.Registration),
                Characters = charactersViewModels,
                Npcs = npcsViewModels
            };
        }
    }
}