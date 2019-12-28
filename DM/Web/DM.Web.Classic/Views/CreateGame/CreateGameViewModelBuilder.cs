using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Implementation;
using DM.Services.Core.Parsing;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.BusinessProcesses.Schemas.Reading;

namespace DM.Web.Classic.Views.CreateGame
{
    public class CreateGameViewModelBuilder : ICreateGameViewModelBuilder
    {
        private readonly IGameReadingService gameService;
        private readonly ISchemaReadingService schemaService;
        private readonly ICreateGameFormBuilder createGameFormBuilder;
        private readonly IBbParserProvider bbParserProvider;

        public CreateGameViewModelBuilder(
            IGameReadingService gameService,
            ISchemaReadingService schemaService,
            ICreateGameFormBuilder createGameFormBuilder,
            IGuidFactory guidFactory,
            IBbParserProvider bbParserProvider
        )
        {
            this.gameService = gameService;
            this.schemaService = schemaService;
            this.createGameFormBuilder = createGameFormBuilder;
            this.bbParserProvider = bbParserProvider;
        }

        public async Task<CreateGameViewModel> Build()
        {
            var schemas = (await schemaService.Get())
                .ToDictionary(s => s.Id, s => s.Name);
            schemas.Add(CreateGameForm.NoSchema, "Без характеристик");
            schemas.Add(Guid.Empty, "Создать новую схему");

            var tags = (await gameService.GetTags())
                .GroupBy(t => t.GroupTitle)
                .ToDictionary(g => g.Key,
                    g => (IDictionary<string, object>) g.ToDictionary(v => v.Title, v => (object) v.Id));

            return new CreateGameViewModel
            {
                AttributeSchemas = schemas,
                Tags = tags,
                CreateGameForm = createGameFormBuilder.Build(schemas.Keys.First()),
                Parser = bbParserProvider.CurrentInfo
            };
        }
    }
}