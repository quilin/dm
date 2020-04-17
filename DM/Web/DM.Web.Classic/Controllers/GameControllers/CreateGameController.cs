using System;
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
    public class CreateGameController : Controller
    {
        private readonly ICreateGameViewModelBuilder createGameViewModelBuilder;
        private readonly IIntentionManager intentionManager;
        private readonly IGameCreatingService gameCreatingService;

        public CreateGameController(
            ICreateGameViewModelBuilder createGameViewModelBuilder,
            IIntentionManager intentionManager,
            IGameCreatingService gameCreatingService)
        {
            this.createGameViewModelBuilder = createGameViewModelBuilder;
            this.intentionManager = intentionManager;
            this.gameCreatingService = gameCreatingService;
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
                Draft = createGameForm.CreateAsRegistration,
                CommentariesAccessMode = createGameForm.CommentariesAccessMode,
                Info = createGameForm.Info,
                DisableAlignment = createGameForm.DisableAlignment,
                HideInventory = createGameForm.HideInventory,
                HideSkills = createGameForm.HideSkills,
                HideStory = createGameForm.HideStory,
                HideTemper = createGameForm.HideTemper,
                HideDiceResult = createGameForm.HideDiceResults,
                ShowPrivateMessages = createGameForm.ShowPrivateMessages,
                AssistantLogin = createGameForm.AssistantLogin,
                AttributeSchemaId = createGameForm.AttributeSchemaId == Guid.Empty
                    ? (Guid?) null
                    : createGameForm.AttributeSchemaId,
                Tags = createGameForm.TagIds
            });
            return RedirectToAction("Index", "Game", new {gameId = game.Id.EncodeToReadable(game.Title)});
        }
    }
}