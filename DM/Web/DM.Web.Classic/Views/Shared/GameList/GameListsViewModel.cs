namespace DM.Web.Classic.Views.Shared.GameList
{
    public class GameListsViewModel
    {
        public bool ForGuest { get; set; }
        public bool OnlyActive { get; set; }

        public PlayerGamesListViewModel MyGames { get; set; }

        public GameListViewModel ActiveGames { get; set; }
        public GameListViewModel RequiringGames { get; set; }
        public GameListViewModel FinishedGames { get; set; }
    }
}