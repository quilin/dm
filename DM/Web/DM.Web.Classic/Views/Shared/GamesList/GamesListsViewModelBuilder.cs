using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Dto.Enums;

namespace DM.Web.Classic.Views.Shared.GamesList
{
    public class GamesListsViewModelBuilder : IGamesListsViewModelBuilder
    {
        private readonly IIdentityProvider identityProvider;
        private readonly IGameListViewModelBuilder gameListViewModelBuilder;

        public GamesListsViewModelBuilder(
            IIdentityProvider identityProvider,
            IGameListViewModelBuilder gameListViewModelBuilder)
        {
            this.identityProvider = identityProvider;
            this.gameListViewModelBuilder = gameListViewModelBuilder;
        }

        public async Task<GameListsViewModel> Build(bool onlyActive, Guid? currentGameId)
        {
            var currentUser = identityProvider.Current.User;
            var ownGames = currentUser.IsAuthenticated
                ? await gameListViewModelBuilder.Build(currentGameId)
                : null;
            var result = new GameListsViewModel
            {
                OnlyActive = onlyActive,
                ForGuest = !currentUser.IsAuthenticated,
                ActiveGames = currentUser.IsAuthenticated
                    ? null
                    : await gameListViewModelBuilder.Build(GameStatus.Active, 10),
                MyGames = ownGames
            };

            if (onlyActive)
            {
                return result;
            }

            result.FinishedGames = await gameListViewModelBuilder.Build(GameStatus.Finished, 10);
            result.RequiringGames = await gameListViewModelBuilder.Build(GameStatus.Requirement, 30);
            if (currentUser.IsAuthenticated && ownGames != null)
            {
                var ownGameIds = ownGames.CharacterGames.Select(g => g.GameId)
                    .Union(ownGames.MasterGames.Select(g => g.GameId))
                    .Union(ownGames.ReaderGames.Select(g => g.GameId))
                    .ToHashSet();
                result.RequiringGames.GameLinks = result.RequiringGames.GameLinks
                    .Where(l => !ownGameIds.Contains(l.GameId));
            }

            return result;
        }
    }
}