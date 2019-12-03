using System;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.GameControllers
{
    public class GameSettingsController : DmControllerBase
    {
        private readonly IModuleSettingsViewModelBuilder moduleSettingsViewModelBuilder;

        public GameSettingsController(
            IModuleSettingsViewModelBuilder moduleSettingsViewModelBuilder
        )
        {
            this.moduleSettingsViewModelBuilder = moduleSettingsViewModelBuilder;
        }

        public IActionResult Index(Guid moduleId, ModuleSettingsType settingsType)
        {
            if (Request.IsAjaxRequest())
            {
                return ViewComponent(nameof(ModuleSettings), new {moduleId, settingsType});
            }

            var moduleSettingsViewModel = moduleSettingsViewModelBuilder.Build(moduleId, settingsType);
            return View("ModuleSettings", moduleSettingsViewModel);
        }
    }
}