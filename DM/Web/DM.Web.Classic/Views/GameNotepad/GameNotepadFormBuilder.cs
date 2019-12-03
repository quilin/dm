using System;
using System.Threading.Tasks;
using DM.Services.Core.Parsing;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;

namespace DM.Web.Classic.Views.GameNotepad
{
    public class GameNotepadFormBuilder : IGameNotepadFormBuilder
    {
        private readonly IGameReadingService gameService;
        private readonly IBbParserProvider bbParserProvider;

        public GameNotepadFormBuilder(
            IGameReadingService gameService,
            IBbParserProvider bbParserProvider)
        {
            this.gameService = gameService;
            this.bbParserProvider = bbParserProvider;
        }

        public async Task<GameNotepadForm> Build(Guid gameId)
        {
            var parser = bbParserProvider.CurrentInfo;
            var game = await gameService.GetGameDetails(gameId);
            var notepad = parser.Parse(game.Notepad);
            return new GameNotepadForm
            {
                ModuleId = gameId,
                Notepad = notepad.ToBb(),
                NotepadHtml = notepad.ToHtml(),
                Parser = parser
            };
        }
    }
}