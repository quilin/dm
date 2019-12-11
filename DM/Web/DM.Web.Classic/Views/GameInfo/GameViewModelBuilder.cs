using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Parsing;
using DM.Services.Gaming.BusinessProcesses.Characters.Reading;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.BusinessProcesses.Readers.Reading;
using DM.Web.Classic.Views.GameInfo.Characters;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.GameInfo
{
    public class GameViewModelBuilder : IGameViewModelBuilder
    {
        private readonly IGameReadingService gameService;
        private readonly ICharacterReadingService characterService;
        private readonly IReadersReadingService readersService;
        private readonly IBbParserProvider bbParserProvider;
        private readonly IUserViewModelBuilder userViewModelBuilder;
        private readonly ICharacterViewModelBuilder characterViewModelBuilder;

        public GameViewModelBuilder(
            IGameReadingService gameService,
            ICharacterReadingService characterService,
            IReadersReadingService readersService,
            IBbParserProvider bbParserProvider,
            IUserViewModelBuilder userViewModelBuilder,
            ICharacterViewModelBuilder characterViewModelBuilder)
        {
            this.gameService = gameService;
            this.characterService = characterService;
            this.readersService = readersService;
            this.bbParserProvider = bbParserProvider;
            this.userViewModelBuilder = userViewModelBuilder;
            this.characterViewModelBuilder = characterViewModelBuilder;
        }

        public async Task<GameViewModel> Build(Guid gameId)
        {
            var game = await gameService.GetGameDetails(gameId);
            var characters = (await characterService.GetCharacters(gameId)).ToArray();

            // we store all the module participants in this list
            // so we won't display readers that are part of the module
            var gameParticipantIds = new List<Guid> {game.Master.UserId};
            if (game.Assistant != null)
            {
                gameParticipantIds.Add(game.Assistant.UserId);
            }

            gameParticipantIds.AddRange(characters.Select(c => c.Author.UserId));

            var readers = (await readersService.Get(gameId))
                .Where(reader => !gameParticipantIds.Contains(reader.UserId))
                .ToArray();

            var charactersViewModels = characters
                .Where(c => !c.IsNpc)
                .Select((c, i) => characterViewModelBuilder.Build(c, i))
                .ToArray();
            var npcsViewModels = characters
                .Where(c => c.IsNpc)
                .Select((c, i) => characterViewModelBuilder.Build(c, i))
                .ToArray();

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
                Assistant = game.Assistant == null
                    ? null
                    : userViewModelBuilder.Build(game.Assistant),

                Readers = readers.Select(userViewModelBuilder.Build).ToArray(),
                Tags = game.Tags.ToArray(),

                ActiveCharactersCount = characters.Count(c => !c.IsNpc && c.Status == CharacterStatus.Active),
                RegisteredCharactersCount = characters.Count(c => !c.IsNpc && c.Status == CharacterStatus.Registration),
                Characters = charactersViewModels,
                Npcs = npcsViewModels
            };
        }
    }
}