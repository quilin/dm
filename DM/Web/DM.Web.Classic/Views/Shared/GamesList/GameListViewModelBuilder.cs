using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.Dto;
using DM.Services.Gaming.Dto.Input;
using DM.Web.Classic.Views.Shared.GamesList.GameLink;

namespace DM.Web.Classic.Views.Shared.GamesList
{
    public class GameListViewModelBuilder : IGameListViewModelBuilder
    {
        private readonly IGameReadingService gameReadingService;
        private readonly IIntentionManager intentionManager;
        private readonly IIdentityProvider identityProvider;
        private readonly IGameLinkViewModelBuilder gameLinkViewModelBuilder;

        public GameListViewModelBuilder(
            IGameReadingService gameReadingService,
            IIntentionManager intentionManager,
            IIdentityProvider identityProvider,
            IGameLinkViewModelBuilder gameLinkViewModelBuilder)
        {
            this.gameReadingService = gameReadingService;
            this.intentionManager = intentionManager;
            this.identityProvider = identityProvider;
            this.gameLinkViewModelBuilder = gameLinkViewModelBuilder;
        }

        public async Task<GameListViewModel> Build(GameStatus gameStatus, int limit) => new GameListViewModel
        {
            GameLinks = (await gameReadingService.GetGames(new GamesQuery
                {
                    Size = limit,
                    Statuses = new HashSet<GameStatus>{gameStatus}
                })).games
                .Select(g => gameLinkViewModelBuilder.Build(g, null))
                .ToArray(),
            Status = gameStatus
        };

        public async Task<PlayerGamesListViewModel> Build(Guid? currentGameId)
        {
            var userId = identityProvider.Current.User.UserId;
            var ownGames = (await gameReadingService.GetOwnGames())
                .Select(g => (Game: g, Participation: g.Participation(userId)))
                .ToArray();
            
            return new PlayerGamesListViewModel
            {
                RequiresPremoderationGames = intentionManager.IsAllowed(GameIntention.SetStatusModeration)
                    ? (await gameReadingService.GetGames(new GamesQuery
                    {
                        Size = 20,
                        Statuses = new HashSet<GameStatus>{GameStatus.RequiresModeration}
                    })).games.Select(g => gameLinkViewModelBuilder.Build(g, currentGameId))
                    : new GameLinkViewModel[0],
                PremoderatingGames = ownGames
                    .Where(g => g.Participation.HasFlag(GameParticipation.Moderator))
                    .Select(g => gameLinkViewModelBuilder.Build(g.Game, currentGameId)),
                MasterGames = ownGames
                    .Where(g => g.Participation.HasFlag(GameParticipation.Authority) &&
                        !g.Participation.HasFlag(GameParticipation.Moderator))
                    .Select(g => gameLinkViewModelBuilder.Build(g.Game, currentGameId)),
                CharacterGames = ownGames
                    .Where(g => g.Participation.HasFlag(GameParticipation.Player) &&
                        !g.Participation.HasFlag(GameParticipation.Moderator) &&
                        !g.Participation.HasFlag(GameParticipation.Authority))
                    .Select(g => gameLinkViewModelBuilder.Build(g.Game, currentGameId)),
                ReaderGames = ownGames
                    .Where(g => g.Participation.HasFlag(GameParticipation.Reader) &&
                        !g.Participation.HasFlag(GameParticipation.Moderator) &&
                        !g.Participation.HasFlag(GameParticipation.Authority) &&
                        !g.Participation.HasFlag(GameParticipation.Reader))
                    .Select(g => gameLinkViewModelBuilder.Build(g.Game, currentGameId))
            };
        }
    }
}