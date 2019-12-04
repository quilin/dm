using System;
using System.Threading.Tasks;

namespace DM.Web.Classic.Views.Shared.GameList
{
    public interface IGamesListsViewModelBuilder
    {
        Task<GameListsViewModel> Build(bool onlyActive, Guid? currentGameId);
    }
}