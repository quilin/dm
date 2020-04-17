using System;
using System.Collections.Generic;
using BBCodeParser;

namespace DM.Web.Classic.Views.CreateGame
{
    public class CreateGameViewModel
    {
        public Dictionary<Guid, string> AttributeSchemes { get; set; }

        public IDictionary<string, IDictionary<string, object>> Tags { get; set; }

        public CreateGameForm CreateGameForm { get; set; }

        public IBbParser Parser { get; set; }

        public static Guid NewSchemaId = Guid.Parse("08f810d0-f12b-4a1c-bb50-97d5bd8fb353");
    }
}