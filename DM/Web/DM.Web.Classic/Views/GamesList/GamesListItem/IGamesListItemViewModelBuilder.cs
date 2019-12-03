using System;
using DM.Services.Gaming.Dto.Output;

namespace DM.Web.Classic.Views.GamesList.GamesListItem
{
    public interface IGamesListItemViewModelBuilder
    {
        GamesListItemViewModel Build(Game game, int number, Guid? tagId);
    }
}