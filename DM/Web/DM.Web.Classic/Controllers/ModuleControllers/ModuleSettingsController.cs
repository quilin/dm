using System;
using DM.Web.Classic.Extensions.RequestExtensions;
using DM.Web.Classic.ViewComponents;
using DM.Web.Classic.Views.ModuleSettings;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.ModuleControllers
{
    public class ModuleSettingsController : DmControllerBase
    {
        private readonly IModuleSettingsViewModelBuilder moduleSettingsViewModelBuilder;

        public ModuleSettingsController(
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