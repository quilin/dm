using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.BusinessProcesses.Games.Updating;
using DM.Services.Gaming.Dto.Input;
using DM.Web.Classic.Middleware;
using DM.Web.Classic.Views.EditGameStatus;
using DM.Web.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Controllers.GameControllers
{
    public class EditGameStatusController : DmControllerBase
    {
        private readonly IGameUpdatingService gameUpdatingService;
        private readonly IEditGameStatusViewModelBuilder editGameStatusViewModelBuilder;

        public EditGameStatusController(
            IGameUpdatingService gameUpdatingService,
            IEditGameStatusViewModelBuilder editGameStatusViewModelBuilder)
        {
            this.gameUpdatingService = gameUpdatingService;
            this.editGameStatusViewModelBuilder = editGameStatusViewModelBuilder;
        }

        [HttpGet]
        public async Task<IActionResult> EditStatus(Guid gameId)
        {
            var editModuleStatusViewModel = await editGameStatusViewModelBuilder.Build(gameId);
            return View("~/Views/EditGameStatus/EditGameStatus.cshtml", editModuleStatusViewModel);
        }

        [HttpPost, ValidationRequired]
        public async Task<IActionResult> EditStatus(EditGameStatusForm form)
        {
            var game = await gameUpdatingService.Update(new UpdateGame
            {
                GameId = form.GameId,
                Status = form.Status
            });
            return Content(Url.Action("Index", "Game",
                new RouteValueDictionary {{"gameId", form.GameId.EncodeToReadable(game.Title)}}));
        }

        [HttpPost]
        public async Task<IActionResult> Premoderate(Guid gameId)
        {
            var game = await gameUpdatingService.Update(new UpdateGame
            {
                GameId = gameId,
                Status = GameStatus.Moderation
            });
            return Content(Url.Action("Index", "Game",
                new RouteValueDictionary {{"gameId", gameId.EncodeToReadable(game.Title)}}));
        }
    }
}