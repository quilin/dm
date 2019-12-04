using DM.Services.Core.Dto.Enums;
using DM.Web.Classic.Views.Shared.GameList.GameLink;

namespace DM.Web.Classic.Views.Shared.GameList
{
    public class GameListViewModel
    {
        public GameLinkViewModel[] GamesLink { get; set; }
        public GameStatus Status { get; set; }
    }
}