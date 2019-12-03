using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Implementation;
using DM.Services.Core.Parsing;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;

namespace DM.Web.Classic.Views.CreateGame
{
    public class CreateGameViewModelBuilder : ICreateGameViewModelBuilder
    {
        private readonly IGameReadingService gameService;
        private readonly ICreateGameFormBuilder createGameFormBuilder;
        private readonly IBbParserProvider bbParserProvider;

        public CreateGameViewModelBuilder(
            IGameReadingService gameService,
            ICreateGameFormBuilder createGameFormBuilder,
            IGuidFactory guidFactory,
            IBbParserProvider bbParserProvider
        )
        {
            this.gameService = gameService;
            this.createGameFormBuilder = createGameFormBuilder;
            this.bbParserProvider = bbParserProvider;
        }

        public async Task<CreateGameViewModel> Build()
        {
            var groupedTags = await gameService.GetTags();
            var tags = groupedTags.GroupBy(t => t.GroupTitle)
                .ToDictionary(g => g.Key,
                    g => (IDictionary<string, object>) g.ToDictionary(v => v.Title, v => (object) v.Id));

            return new CreateGameViewModel
            {
                Tags = tags,
                CreateGameForm = createGameFormBuilder.Build(),
                Parser = bbParserProvider.CurrentInfo
            };
        }
    }
}