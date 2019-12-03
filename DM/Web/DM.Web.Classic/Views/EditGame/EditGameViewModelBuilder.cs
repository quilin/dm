using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.Dto.Output;

namespace DM.Web.Classic.Views.EditGame
{
    public class EditGameViewModelBuilder : IEditGameViewModelBuilder
    {
        private readonly IGameReadingService gameService;
        private readonly IEditGameFormBuilder editGameFormBuilder;

        public EditGameViewModelBuilder(
            IGameReadingService gameService,
            IEditGameFormBuilder editGameFormBuilder)
        {
            this.gameService = gameService;
            this.editGameFormBuilder = editGameFormBuilder;
        }

        public async Task<EditGameViewModel> Build(GameExtended game)
        {
            var groupedTags = await gameService.GetTags();
            var tags = groupedTags
                .GroupBy(t => t.GroupTitle)
                .ToDictionary(g => g.Key, g =>
                    (IDictionary<string, object>) g.ToDictionary(v => v.Title, v => (object) v.Id));

            return new EditGameViewModel
            {
                Assistant = game.Assistant?.Login,
                Tags = tags,
                EditGameForm = editGameFormBuilder.Build(game)
            };
        }
    }
}