using System.Collections.Generic;
using DM.Services.Core.Dto.Enums;
using DM.Web.Classic.Views.Shared.GamesList.GameLink;

namespace DM.Web.Classic.Views.Shared.GamesList
{
    public class GameListViewModel
    {
        public IEnumerable<GameLinkViewModel> GameLinks { get; set; }
        public GameStatus Status { get; set; }
    }
}