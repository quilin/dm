using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Web.Classic.Extensions.RequestExtensions;
using DM.Web.Classic.Views.GamesList;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.GameControllers
{
    public class GamesListController : DmControllerBase
    {
        private readonly IGamesListViewModelBuilder gamesListViewModelBuilder;

        public GamesListController(
            IGamesListViewModelBuilder gamesListViewModelBuilder
            )
        {
            this.gamesListViewModelBuilder = gamesListViewModelBuilder;
        }

        public async Task<IActionResult> Index(GameStatus status, int entityNumber)
        {
            if (!Request.IsAjaxRequest())
            {
                var modulesListViewModel = await gamesListViewModelBuilder.Build(status, entityNumber);
                return View("Index", modulesListViewModel);
            }
            var (modulesListItemViewModels, _) = await gamesListViewModelBuilder.BuildList(status, entityNumber);
            return PartialView("~/Views/GamesList/GamesList.cshtml", modulesListItemViewModels);
        }

        public async Task<IActionResult> Tags(Guid tagId, int entityNumber)
        {
            if (!Request.IsAjaxRequest())
            {
                var modulesListViewModel = await gamesListViewModelBuilder.Build(tagId, entityNumber);
                return View("Index", modulesListViewModel);
            }
            var (modulesListItemViewModels, _) = await gamesListViewModelBuilder.BuildList(tagId, entityNumber);
            return PartialView("~/Views/GamesList/GamesList.cshtml", modulesListItemViewModels);
        }
    }
}