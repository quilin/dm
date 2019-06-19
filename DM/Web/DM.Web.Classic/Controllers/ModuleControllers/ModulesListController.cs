using System;
using DM.Services.Core.Dto.Enums;
using DM.Web.Classic.Extensions.RequestExtensions;
using DM.Web.Classic.Views.ModulesList;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.ModuleControllers
{
    public class ModulesListController : DmControllerBase
    {
        private readonly IModulesListViewModelBuilder modulesListViewModelBuilder;

        public ModulesListController(
            IModulesListViewModelBuilder modulesListViewModelBuilder
            )
        {
            this.modulesListViewModelBuilder = modulesListViewModelBuilder;
        }

        public IActionResult Index(GameStatus status, int entityNumber)
        {
            if (!Request.IsAjaxRequest())
            {
                var modulesListViewModel = modulesListViewModelBuilder.Build(status, entityNumber);
                return View("Index", modulesListViewModel);
            }
            var modulesListItemViewModels = modulesListViewModelBuilder.BuildList(status, entityNumber);
            return PartialView("~/Views/ModulesList/ModulesList.cshtml", modulesListItemViewModels);
        }

        public IActionResult Tags(Guid tagId, int entityNumber)
        {
            if (!Request.IsAjaxRequest())
            {
                var modulesListViewModel = modulesListViewModelBuilder.Build(tagId, entityNumber);
                return View("Index", modulesListViewModel);
            }
            var modulesListItemViewModels = modulesListViewModelBuilder.BuildList(tagId, entityNumber);
            return PartialView("~/Views/ModulesList/ModulesList.cshtml", modulesListItemViewModels);
        }
    }
}