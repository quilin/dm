using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Web.Classic.Views.GamesList.GamesListItem;

namespace DM.Web.Classic.Views.GamesList
{
    public interface IGamesListViewModelBuilder
    {
        Task<GamesListViewModel> Build(GameStatus status, int entityNumber);
        Task<(IEnumerable<GamesListItemViewModel> games, PagingResult paging)> BuildList(
            GameStatus status, int entityNumber);

        Task<GamesListViewModel> Build(Guid tagId, int entityNumber);
        Task<(IEnumerable<GamesListItemViewModel> games, PagingResult paging)> BuildList(
            Guid tagId, int entityNumber);
    }
}