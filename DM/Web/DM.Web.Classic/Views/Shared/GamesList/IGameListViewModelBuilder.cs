using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;

namespace DM.Web.Classic.Views.Shared.GamesList
{
    public interface IGameListViewModelBuilder
    {
        Task<GameListViewModel> Build(GameStatus gameStatus, int limit);
        Task<PlayerGamesListViewModel> Build(Guid? currentGameId);
    }
}