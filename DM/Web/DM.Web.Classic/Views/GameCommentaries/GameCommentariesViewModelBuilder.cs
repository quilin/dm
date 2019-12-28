using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.BusinessProcesses.Characters.Reading;
using DM.Services.Gaming.BusinessProcesses.Commentaries.Reading;
using DM.Services.Gaming.Dto.Output;
using DM.Web.Classic.Views.GameCommentaries.Commentary;
using DM.Web.Classic.Views.GameCommentaries.CreateCommentary;

namespace DM.Web.Classic.Views.GameCommentaries
{
    public class GameCommentariesViewModelBuilder : IGameCommentariesViewModelBuilder
    {
        private readonly ICommentaryReadingService commentService;
        private readonly ICharacterReadingService characterService;
        private readonly IGameCommentaryViewModelBuilder gameCommentaryViewModelBuilder;
        private readonly IGameCommentaryCreateFormBuilder gameCommentaryCreateFormBuilder;

        public GameCommentariesViewModelBuilder(
            ICommentaryReadingService commentService,
            ICharacterReadingService characterService,
            IGameCommentaryViewModelBuilder gameCommentaryViewModelBuilder,
            IGameCommentaryCreateFormBuilder gameCommentaryCreateFormBuilder)
        {
            this.commentService = commentService;
            this.characterService = characterService;
            this.gameCommentaryViewModelBuilder = gameCommentaryViewModelBuilder;
            this.gameCommentaryCreateFormBuilder = gameCommentaryCreateFormBuilder;
        }

        public async Task<GameCommentariesViewModel> Build(Guid gameId, int entityNumber)
        {
            var (comments, paging, game) = await commentService.Get(gameId, new PagingQuery {Number = entityNumber});
            var characterNames = await GetCharacterNames(game);
            return new GameCommentariesViewModel
            {
                GameId = game.Id,
                GameTitle = game.Title,
                TotalPagesCount = paging.TotalPagesCount,
                CurrentPage = paging.CurrentPage,
                PageSize = paging.PageSize,
                EntityNumber = paging.EntityNumber,
                ModuleCommentaries = comments.Select(c =>
                    gameCommentaryViewModelBuilder.Build(c, game, characterNames).Result),
                CreateForm = gameCommentaryCreateFormBuilder.Build(game)
            };
        }

        public async Task<IEnumerable<GameCommentaryViewModel>> BuildList(Guid gameId, int entityNumber)
        {
            var (comments, _, game) = await commentService.Get(gameId, new PagingQuery {Number = entityNumber});
            var characterNames = await GetCharacterNames(game);
            return comments.Select(c => gameCommentaryViewModelBuilder.Build(c, game, characterNames).Result);
        }

        private async Task<IDictionary<Guid, IEnumerable<string>>> GetCharacterNames(Game game)
        {
            var characterNames = (await characterService.GetCharacters(game.Id))
                .Where(c => c.Status == CharacterStatus.Registration || c.Status == CharacterStatus.Active)
                .GroupBy(c => c.Author.UserId)
                .ToDictionary(g => g.Key, g => g.Select(c => c.Name));

            characterNames[game.Master.UserId] = new[] {"DungeonMaster"};
            if (game.Assistant != null)
            {
                characterNames[game.Assistant.UserId] = new[] {"Assistant"};
            }

            return characterNames;
        }
    }
}