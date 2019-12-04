using System;
using System.Threading.Tasks;
using DM.Web.Classic.Views.Shared.GameList;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.ViewComponents.LeftBar
{
    public class GamesLists : ViewComponent
    {
        private readonly IGamesListsViewModelBuilder gamesListsViewModelBuilder;

        public GamesLists(
            IGamesListsViewModelBuilder gamesListsViewModelBuilder)
        {
            this.gamesListsViewModelBuilder = gamesListsViewModelBuilder;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(bool? onlyActive, Guid currentModuleId)
        {
            var moduleListsViewModel = await gamesListsViewModelBuilder.Build(onlyActive ?? false, currentModuleId);
            return View("~/Views/Shared/GameList/GamesLists.cshtml", moduleListsViewModel);
        }
    }
}