using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.Dto.Input;
using DM.Web.Classic.Views.GamesList.GamesListItem;

namespace DM.Web.Classic.Views.GamesList
{
    public class GamesListViewModelBuilder : IGamesListViewModelBuilder
    {
        private readonly IGameReadingService gameService;
        private readonly IGamesListItemViewModelBuilder gamesListItemViewModelBuilder;

        public GamesListViewModelBuilder(
            IGameReadingService gameService,
            IGamesListItemViewModelBuilder gamesListItemViewModelBuilder
        )
        {
            this.gameService = gameService;
            this.gamesListItemViewModelBuilder = gamesListItemViewModelBuilder;
        }

        private const int PageSize = 20;

        public async Task<GamesListViewModel> Build(GameStatus status, int entityNumber)
        {
            var (games, paging) = await BuildList(status, entityNumber);
            return new GamesListViewModel
            {
                Status = status,
                PageTitle = PageTitles[status],
                CurrentPage = paging.CurrentPage,
                TotalPagesCount = paging.TotalPagesCount,
                PageSize = paging.PageSize,
                EntityNumber = paging.EntityNumber,
                Games = games
            };
        }

        public async Task<(IEnumerable<GamesListItemViewModel> games, PagingResult paging)> BuildList(
            GameStatus status, int entityNumber)
        {
            var query = new GamesQuery {Number = entityNumber, Size = PageSize, Status = status};
            var (games, paging) = await gameService.GetGames(query);

            return (games.Select((m, i) => gamesListItemViewModelBuilder.Build(
                m, (paging.CurrentPage - 1) * PageSize + 1 + i, null)), paging);
        }

        private static readonly IDictionary<GameStatus, string> PageTitles = new Dictionary<GameStatus, string>
        {
            {GameStatus.Active, "Активные игры"},
            {GameStatus.Closed, "Закрытые игры"},
            {GameStatus.Finished, "Завершенные игры"},
            {GameStatus.Frozen, "Замороженные игры"},
            {GameStatus.Requirement, "Игры с открытым набором"}
        };

        public async Task<GamesListViewModel> Build(Guid tagId, int entityNumber)
        {
            var (games, paging) = await BuildList(tagId, entityNumber);
            var tagTitle = (await gameService.GetTags()).First(t => t.Id == tagId).Title;
            return new GamesListViewModel
            {
                Status = null,
                PageTitle = $"Поиск игр по тегу \"{tagTitle}\"",
                CurrentPage = paging.CurrentPage,
                TotalPagesCount = paging.TotalPagesCount,
                PageSize = paging.PageSize,
                EntityNumber = paging.EntityNumber,
                Games = games
            };
        }

        public async Task<(IEnumerable<GamesListItemViewModel> games, PagingResult paging)> BuildList(
            Guid tagId, int entityNumber)
        {
            var query = new GamesQuery {Number = entityNumber, Size = PageSize, TagId = tagId};
            var (games, paging) = await gameService.GetGames(query);
            return (games.Select((m, i) => gamesListItemViewModelBuilder.Build(
                m, (paging.CurrentPage - 1) * PageSize + 1 + i, tagId)), paging);
        }
    }
}