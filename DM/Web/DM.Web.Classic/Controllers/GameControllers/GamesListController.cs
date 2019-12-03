using System;
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

        public IActionResult Index(ModuleStatus status, int entityNumber)
        {
            if (!Request.IsAjaxRequest())
            {
                var modulesListViewModel = gamesListViewModelBuilder.Build(status, entityNumber);
                return View("Index", modulesListViewModel);
            }
            var modulesListItemViewModels = gamesListViewModelBuilder.BuildList(status, entityNumber);
            return PartialView("~/Views/ModulesList/ModulesList.cshtml", modulesListItemViewModels);
        }

        public IActionResult Tags(Guid tagId, int entityNumber)
        {
            if (!Request.IsAjaxRequest())
            {
                var modulesListViewModel = gamesListViewModelBuilder.Build(tagId, entityNumber);
                return View("Index", modulesListViewModel);
            }
            var modulesListItemViewModels = gamesListViewModelBuilder.BuildList(tagId, entityNumber);
            return PartialView("~/Views/ModulesList/ModulesList.cshtml", modulesListItemViewModels);
        }
    }
}