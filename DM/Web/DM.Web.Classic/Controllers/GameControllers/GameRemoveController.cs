using System;
using System.Threading.Tasks;
using DM.Services.Gaming.BusinessProcesses.Games.Deleting;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.GameControllers
{
    public class GameRemoveController : DmControllerBase
    {
        private readonly IGameDeletingService gameDeletingService;

        public GameRemoveController(
            IGameDeletingService gameDeletingService)
        {
            this.gameDeletingService = gameDeletingService;
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid gameId)
        {
            await gameDeletingService.DeleteGame(gameId);
            return RedirectToAction("Index", "Home");
        }
    }
}