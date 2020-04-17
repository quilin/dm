using System;

namespace DM.Web.Classic.Views.CreateGame
{
    public interface ICreateGameFormBuilder
    {
        CreateGameForm Build(Guid attributeSchemeId);
    }
}