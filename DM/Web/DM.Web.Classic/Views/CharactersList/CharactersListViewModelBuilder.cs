using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Gaming.BusinessProcesses.Characters.Reading;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;

namespace DM.Web.Classic.Views.CharactersList
{
    public class CharactersListViewModelBuilder : ICharactersListViewModelBuilder
    {
        private readonly IGameReadingService gameService;
        private readonly ICharacterReadingService characterService;
        private readonly ICharactersSublistViewModelBuilder charactersSublistViewModelBuilder;

        public CharactersListViewModelBuilder(
            IGameReadingService gameService,
            ICharacterReadingService characterService,
            ICharactersSublistViewModelBuilder charactersSublistViewModelBuilder)
        {
            this.gameService = gameService;
            this.characterService = characterService;
            this.charactersSublistViewModelBuilder = charactersSublistViewModelBuilder;
        }

        public async Task<CharactersListViewModel> Build(Guid gameId, Guid? characterId)
        {
            var game = await gameService.GetGameDetails(gameId);
            var characters = (await characterService.GetCharacters(gameId)).ToArray();

            var charactersLists = new[]
                {
                    charactersSublistViewModelBuilder.BuildPlayer(characters, game, characterId),
                    charactersSublistViewModelBuilder.BuildNpc(characters, game, characterId),
                    charactersSublistViewModelBuilder.BuildOutOfGame(characters, game, characterId),
                    charactersSublistViewModelBuilder.BuildDeclined(characters, game, characterId)
                }
                .Where(list => !list.IsEmpty)
                .ToArray();

            if (charactersLists.Any() && !charactersLists.Any(s => s.IsDefault))
            {
                charactersLists.First().IsDefault = true;
            }

            return new CharactersListViewModel
            {
                GameId = gameId,
                CharacterId = characterId,
                GameTitle = game.Title,
                CharacterLists = charactersLists
            };
        }
    }
}