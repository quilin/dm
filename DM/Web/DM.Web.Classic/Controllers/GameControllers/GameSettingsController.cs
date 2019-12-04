using System;
using System.Threading.Tasks;
using DM.Web.Classic.Extensions.RequestExtensions;
using DM.Web.Classic.ViewComponents;
using DM.Web.Classic.Views.GameSettings;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.GameControllers
{
    public class GameSettingsController : DmControllerBase
    {
        private readonly IGameSettingsViewModelBuilder gameSettingsViewModelBuilder;

        public GameSettingsController(
            IGameSettingsViewModelBuilder gameSettingsViewModelBuilder
        )
        {
            this.gameSettingsViewModelBuilder = gameSettingsViewModelBuilder;
        }

        public async Task<IActionResult> Index(Guid moduleId, GameSettingsType settingsType)
        {
            if (Request.IsAjaxRequest())
            {
                return ViewComponent(nameof(GameSettings), new {moduleId, settingsType});
            }

            var moduleSettingsViewModel = await gameSettingsViewModelBuilder.Build(moduleId, settingsType);
            return View("GameSettings", moduleSettingsViewModel);
        }
    }
}