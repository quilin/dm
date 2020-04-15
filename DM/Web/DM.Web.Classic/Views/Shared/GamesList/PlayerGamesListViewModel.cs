using System.Collections.Generic;
using DM.Web.Classic.Views.Shared.GamesList.GameLink;

namespace DM.Web.Classic.Views.Shared.GamesList
{
    public class PlayerGamesListViewModel
    {
        public IEnumerable<GameLinkViewModel> RequiresPremoderationGames { get; set; }
        public IEnumerable<GameLinkViewModel> PremoderatingGames { get; set; }
        public IEnumerable<GameLinkViewModel> MasterGames { get; set; }
        public IEnumerable<GameLinkViewModel> CharacterGames { get; set; }
        public IEnumerable<GameLinkViewModel> ReaderGames { get; set; }
    }
}