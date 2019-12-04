using System.Threading.Tasks;
using DM.Services.Core.Parsing;
using DM.Services.Gaming.BusinessProcesses.Games.Updating;
using DM.Services.Gaming.Dto.Input;
using DM.Web.Classic.Views.GameNotepad;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.GameControllers
{
    public class GameNotepadController : DmControllerBase
    {
        private readonly IGameUpdatingService updatingService;
        private readonly IBbParserProvider bbParserProvider;

        public GameNotepadController(
            IGameUpdatingService updatingService,
            IBbParserProvider bbParserProvider)
        {
            this.updatingService = updatingService;
            this.bbParserProvider = bbParserProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GameNotepadForm form)
        {
            var game = await updatingService.Update(new UpdateGame
            {
                GameId = form.GameId,
                Notepad = form.Notepad
            });
            return Content(bbParserProvider.CurrentInfo.Parse(game.Notepad).ToHtml());
        }
    }
}