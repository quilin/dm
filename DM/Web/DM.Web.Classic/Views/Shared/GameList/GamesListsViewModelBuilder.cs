using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Dto.Enums;

namespace DM.Web.Classic.Views.Shared.GameList
{
    public class GamesListsViewModelBuilder : IGamesListsViewModelBuilder
    {
        private readonly IGameListViewModelBuilder gameListViewModelBuilder;
        private readonly IIdentity identity;

        public GamesListsViewModelBuilder(
            IGameListViewModelBuilder gameListViewModelBuilder,
            IIdentityProvider identityProvider)
        {
            this.gameListViewModelBuilder = gameListViewModelBuilder;
            identity = identityProvider.Current;
        }

        public async Task<GameListsViewModel> Build(bool onlyActive, Guid? currentGameId)
        {
            var requiringGames = onlyActive
                ? null
                : await gameListViewModelBuilder.Build(GameStatus.Requirement, 20);
            var finishedGames = onlyActive
                ? null
                : await gameListViewModelBuilder.Build(GameStatus.Finished, 10);

            if (!identity.User.IsAuthenticated)
            {
                return new GameListsViewModel
                {
                    ForGuest = true,
                    OnlyActive = onlyActive,
                    ActiveGames = await gameListViewModelBuilder.Build(GameStatus.Active, 10),
                    RequiringGames = requiringGames,
                    FinishedGames = finishedGames,
                    MyGames = null
                };
            }

            var userGames = await gameListViewModelBuilder.Build(currentGameId);
            if (requiringGames != null)
            {
                var userGameIds = new List<Guid>();
                userGameIds.AddRange(userGames.CharacterGames.Select(g => g.GameId));
                userGameIds.AddRange(userGames.MasterGames.Select(g => g.GameId));
                userGameIds.AddRange(userGames.ReaderGames.Select(g => g.GameId));
                requiringGames.GamesLink = requiringGames.GamesLink
                    .Where(l => !userGameIds.Contains(l.GameId))
                    .ToArray();
            }

            return new GameListsViewModel
            {
                ForGuest = false,
                OnlyActive = onlyActive,
                ActiveGames = null,
                RequiringGames = requiringGames,
                FinishedGames = finishedGames,
                MyGames = userGames
            };
        }
    }
}