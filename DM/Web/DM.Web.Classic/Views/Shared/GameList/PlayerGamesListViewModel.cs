using DM.Web.Classic.Views.Shared.GameList.GameLink;

namespace DM.Web.Classic.Views.Shared.GameList
{
    public class PlayerGamesListViewModel
    {
        public GameLinkViewModel[] RequiresPremoderationModules { get; set; }
        public GameLinkViewModel[] PremoderatingModules { get; set; }
        public GameLinkViewModel[] MasterGames { get; set; }
        public GameLinkViewModel[] CharacterGames { get; set; }
        public GameLinkViewModel[] ReaderGames { get; set; }
    }
}