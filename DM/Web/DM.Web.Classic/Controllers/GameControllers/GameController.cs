using System;
using System.Threading.Tasks;
using DM.Services.Gaming.BusinessProcesses.Readers.Subscribing;
using DM.Web.Classic.Views.GameInfo;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.GameControllers
{
    public class GameController : Controller
    {
        private readonly IGameViewModelBuilder gameViewModelBuilder;
        private readonly IReadingSubscribingService readingSubscribingService;

        public GameController(
            IGameViewModelBuilder gameViewModelBuilder,
            IReadingSubscribingService readingSubscribingService)
        {
            this.gameViewModelBuilder = gameViewModelBuilder;
            this.readingSubscribingService = readingSubscribingService;
        }
    
        public async Task<IActionResult> Index(Guid gameId)
        {
            var moduleViewModel = await gameViewModelBuilder.Build(gameId);
            return View("~/Views/GameInfo/Game.cshtml", moduleViewModel);
        }

        public async Task<IActionResult> Observe(Guid gameId)
        {
            await readingSubscribingService.Subscribe(gameId);
            return RedirectToAction(Request.GetDisplayUrl());
        }

        public async Task<IActionResult> StopObserving(Guid gameId)
        {
            await readingSubscribingService.Unsubscribe(gameId);
            return Redirect(Request.GetDisplayUrl());
        }
    }
}