using System;
using System.Threading.Tasks;

namespace DM.Web.Classic.Views.Shared.GamesList
{
    public interface IGamesListsViewModelBuilder
    {
        Task<GameListsViewModel> Build(bool onlyActive, Guid? currentModuleId);
    }
}