using System;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.GameControllers
{
    public class GameRemoveController : DmControllerBase
    {
        private readonly IModuleService moduleService;

        public GameRemoveController(
            IModuleService moduleService)
        {
            this.moduleService = moduleService;
        }

        [HttpPost]
        public ActionResult Remove(Guid moduleId)
        {
            moduleService.Remove(moduleId);
            return RedirectToAction("Index", "Home");
        }
    }
}