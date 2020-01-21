using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Core.Extensions;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Creating;
using DM.Services.Gaming.Dto.Input;
using DM.Web.Classic.Middleware;
using DM.Web.Classic.Views.CreateGame;
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
            intentionManager.ThrowIfForbidden(GameIntention.Create);
            var createModuleViewModel = await createGameViewModelBuilder.Build();
            return View("CreateGame", createModuleViewModel);
        }

        [HttpPost, ValidationRequired]
        public async Task<IActionResult> Create(CreateGameForm createGameForm)
        {
            var game = await gameCreatingService.Create(new CreateGame
            {
                Title = createGameForm.Title,
                SystemName = createGameForm.SystemName,
                SettingName = createGameForm.SettingName,
                Info = createGameForm.Info,
                AttributeSchemaId = createGameForm.SchemaId,
                CommentariesAccessMode = createGameForm.CommentariesAccessMode,
                Draft = createGameForm.CreateAsRegistration,
                HideTemper = createGameForm.HideTemper,
                HideStory = createGameForm.HideStory,
                HideSkills = createGameForm.HideSkills,
                HideInventory = createGameForm.HideInventory,
                HideDiceResult = createGameForm.HideDiceResults,
                ShowPrivateMessages = createGameForm.ShowPrivateMessages,
                DisableAlignment = createGameForm.DisableAlignment
            });
            return RedirectToAction("Index", "Game", new {gameId = game.Id.EncodeToReadable(game.Title)});
        }
    }
}