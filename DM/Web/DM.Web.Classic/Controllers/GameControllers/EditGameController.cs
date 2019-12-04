using System.Threading.Tasks;
using DM.Services.Gaming.BusinessProcesses.Games.Updating;
using DM.Services.Gaming.Dto.Input;
using DM.Web.Classic.Middleware;
using DM.Web.Classic.Views.EditGame;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.GameControllers
{
    public class EditGameController : DmControllerBase
    {
        private readonly IGameUpdatingService gameUpdatingService;

        public EditGameController(
            IGameUpdatingService gameUpdatingService)
        {
            this.gameUpdatingService = gameUpdatingService;
        }

        [HttpPost, ValidationRequired]
        public async Task<IActionResult> SaveChanges(EditGameForm editGameForm)
        {
            await gameUpdatingService.Update(new UpdateGame());
            return RedirectToAction("Index", "Game", new {gameId = editGameForm.GameId});
        }
    }
}