using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Parsing;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.BusinessProcesses.Schemas.Reading;

namespace DM.Web.Classic.Views.CreateGame
{
    public class CreateGameViewModelBuilder : ICreateGameViewModelBuilder
    {
        private readonly ICreateGameFormBuilder createGameFormBuilder;
        private readonly ISchemaReadingService schemaReadingService;
        private readonly IGameReadingService gameReadingService;
        private readonly IBbParserProvider bbParserProvider;

        public CreateGameViewModelBuilder(
            ICreateGameFormBuilder createGameFormBuilder,
            ISchemaReadingService schemaReadingService,
            IGameReadingService gameReadingService,
            IBbParserProvider bbParserProvider)
        {
            this.createGameFormBuilder = createGameFormBuilder;
            this.schemaReadingService = schemaReadingService;
            this.gameReadingService = gameReadingService;
            this.bbParserProvider = bbParserProvider;
        }

        public async Task<CreateGameViewModel> Build()
        {
            var schemata = (await schemaReadingService.Get())
                .ToDictionary(s => s.Id, s => s.Title);
            schemata.Add(Guid.Empty, "Без характеристик");
            schemata.Add(CreateGameViewModel.NewSchemaId, "Создать новую схему...");

            var groupedTags = await gameReadingService.GetTags();
            var tags = groupedTags.GroupBy(t => t.GroupTitle)
                .ToDictionary(g => g.Key,
                    g => (IDictionary<string, object>) g.ToDictionary(v => v.Title, v => (object) v.Id));

            return new CreateGameViewModel
            {
                AttributeSchemes = schemata,
                Tags = tags,
                CreateGameForm = createGameFormBuilder.Build(Guid.Empty),
                Parser = bbParserProvider.CurrentInfo
            };
        }
    }
}