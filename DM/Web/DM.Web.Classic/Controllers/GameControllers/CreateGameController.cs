using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Creating;
using DM.Services.Gaming.Dto.Input;
using DM.Web.Classic.Middleware;
using DM.Web.Classic.Views.CreateGame;
using DM.Web.Core.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.GameControllers
{
    public class CreateGameController : DmControllerBase
    {
        private readonly IIntentionManager intentionManager;
        private readonly IGameCreatingService gameCreatingService;
        private readonly ICreateGameViewModelBuilder createGameViewModelBuilder;

        public CreateGameController(
            IIntentionManager intentionManager,
            IGameCreatingService gameCreatingService,
            ICreateGameViewModelBuilder createGameViewModelBuilder)
        {
            this.intentionManager = intentionManager;
            this.gameCreatingService = gameCreatingService;
            this.createGameViewModelBuilder = createGameViewModelBuilder;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await intentionManager.ThrowIfForbidden(GameIntention.Create);
            var createModuleViewModel = await createGameViewModelBuilder.Build();
            return View("CreateGame", createModuleViewModel);
        }

        [HttpPost, ValidationRequired]
        public async Task<IActionResult> Create(CreateGameForm createGameForm)
        {
            var game = await gameCreatingService.Create(new CreateGame());
            return RedirectToAction("Index", "Game", new {moduleId = game.Id.EncodeToReadable(game.Title)});
        }
    }
}