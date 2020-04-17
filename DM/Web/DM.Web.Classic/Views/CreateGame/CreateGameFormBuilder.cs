using System;

namespace DM.Web.Classic.Views.CreateGame
{
    public class CreateGameFormBuilder : ICreateGameFormBuilder
    {
        public CreateGameForm Build(Guid attributeSchemeId)
        {
            return new CreateGameForm
                       {
                           AttributeSchemaId = attributeSchemeId,
                           CreateAsRegistration = true
                       };
        }
    }
}