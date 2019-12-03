using System;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.GameControllers
{
    public class GameBlackListController : DmControllerBase
    {
        private readonly IUserService userService;
        private readonly IModuleService moduleService;
        private readonly IModuleBlackListEntryViewModelBuilder moduleBlackListEntryViewModelBuilder;

        public GameBlackListController(
            IUserService userService,
            IModuleService moduleService,
            IModuleBlackListEntryViewModelBuilder moduleBlackListEntryViewModelBuilder)
        {
            this.userService = userService;
            this.moduleService = moduleService;
            this.moduleBlackListEntryViewModelBuilder = moduleBlackListEntryViewModelBuilder;
        }

        [HttpPost]
        public ActionResult Create(Guid moduleId, string login)
        {
            var user = userService.Find(login);
            moduleService.CreateBlackListLink(moduleId, user.UserId);
            var moduleBlackListEntryViewModel = moduleBlackListEntryViewModelBuilder.Build(user.Login, moduleId);
            return PartialView("ModuleBlackListEntry", moduleBlackListEntryViewModel);
        }

        [HttpPost]
        public void Remove(Guid moduleId, string login)
        {
            var user = userService.Find(login);
            moduleService.RemoveBlackListLink(moduleId, user.UserId);
        }
    }
}