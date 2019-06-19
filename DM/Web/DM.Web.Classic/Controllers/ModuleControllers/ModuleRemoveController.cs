using System;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.ModuleControllers
{
    public class ModuleRemoveController : DmControllerBase
    {
        private readonly IModuleService moduleService;

        public ModuleRemoveController(
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