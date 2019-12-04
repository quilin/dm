using System;
using DM.Services.Gaming.Dto.Output;

namespace DM.Web.Classic.Views.Shared.GameList.GameLink
{
    public interface IGameLinkViewModelBuilder
    {
        GameLinkViewModel Build(Game game, Guid? exceptGameId);
        GameLinkViewModel BuildWithoutCounters(Game game);
    }
}