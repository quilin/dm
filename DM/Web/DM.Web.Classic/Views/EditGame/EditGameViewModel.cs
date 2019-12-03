using System;
using System.Collections.Generic;

namespace DM.Web.Classic.Views.EditGame
{
    public class EditGameViewModel
    {
        public Dictionary<Guid, string> AttributeSchemes { get; set; }
        public Dictionary<Guid, string> AttributeSchemeDescriptions { get; set; }

        public string Assistant { get; set; }

        public IDictionary<string, IDictionary<string, object>> Tags { get; set; }

        public EditGameForm EditGameForm { get; set; }
    }
}