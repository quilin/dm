using System;
using System.Threading.Tasks;
using DM.Services.Gaming.BusinessProcesses.Readers.Subscribing;
using DM.Web.Classic.Views.GameInfo;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.GameControllers
{
    public class GameController : DmControllerBase
    {
        private readonly IReadingSubscribingService subscribingService;
        private readonly IGameViewModelBuilder gameViewModelBuilder;

        public GameController(
            IReadingSubscribingService subscribingService,
            IGameViewModelBuilder gameViewModelBuilder)
        {
            this.subscribingService = subscribingService;
            this.gameViewModelBuilder = gameViewModelBuilder;
        }

        public async Task<IActionResult> Index(Guid gameId)
        {
            var gameViewModel = await gameViewModelBuilder.Build(gameId);
            return View("~/Views/GameInfo/Game.cshtml", gameViewModel);
        }

        public async Task<IActionResult> Observe(Guid gameId)
        {
            await subscribingService.Subscribe(gameId);
            return RedirectToAction(Request.GetDisplayUrl());
        }

        public async Task<IActionResult> StopObserving(Guid gameId)
        {
            await subscribingService.Unsubscribe(gameId);
            return Redirect(Request.GetDisplayUrl());
        }
    }
}