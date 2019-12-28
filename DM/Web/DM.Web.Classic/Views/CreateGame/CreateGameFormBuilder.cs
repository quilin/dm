using System;

namespace DM.Web.Classic.Views.CreateGame
{
    public class CreateGameFormBuilder : ICreateGameFormBuilder
    {
        public CreateGameForm Build(Guid schemaId)
        {
            return new CreateGameForm
            {
                AttributeSchemaId = schemaId,
                CreateAsRegistration = true
            };
        }
    }
}