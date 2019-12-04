using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.Dto;
using DM.Services.Gaming.Dto.Input;
using DM.Web.Classic.Views.Shared.GameList.GameLink;

namespace DM.Web.Classic.Views.Shared.GameList
{
    public class GameListViewModelBuilder : IGameListViewModelBuilder
    {
        private readonly IGameReadingService gameReadingService;
        private readonly IGameLinkViewModelBuilder gameLinkViewModelBuilder;
        private readonly IIdentity identity;

        public GameListViewModelBuilder(
            IGameReadingService gameReadingService,
            IGameLinkViewModelBuilder gameLinkViewModelBuilder,
            IIdentityProvider identityProvider)
        {
            this.gameReadingService = gameReadingService;
            this.gameLinkViewModelBuilder = gameLinkViewModelBuilder;
            identity = identityProvider.Current;
        }

        public async Task<GameListViewModel> Build(GameStatus gameStatus, int limit)
        {
            var (games, _) = await gameReadingService.GetGames(new GamesQuery
            {
                Statuses = new HashSet<GameStatus>{gameStatus},
                Size = limit
            });
            return new GameListViewModel
            {
                GamesLink = games
                    .Select(gameLinkViewModelBuilder.BuildWithoutCounters)
                    .ToArray(),
                Status = gameStatus
            };
        }

        public async Task<PlayerGamesListViewModel> Build(Guid? currentGameId)
        {
            var userId = identity.User.UserId;
            var ownGames = (await gameReadingService.GetOwnGames())
                .ToLookup(g =>
                {
                    var participation = g.Participation(userId);
                    if (participation.HasFlag(GameParticipation.Authority) ||
                        participation.HasFlag(GameParticipation.PendingAssistant))
                    {
                        return GameParticipation.Authority;
                    }

                    if (participation.HasFlag(GameParticipation.Moderator))
                    {
                        return GameParticipation.Moderator;
                    }

                    if (participation.HasFlag(GameParticipation.Player))
                    {
                        return GameParticipation.Player;
                    }

                    if (participation.HasFlag(GameParticipation.Reader))
                    {
                        return GameParticipation.Reader;
                    }

                    return GameParticipation.None;
                });

            return new PlayerGamesListViewModel
            {
                PremoderatingModules = ownGames[GameParticipation.Moderator]
                    .Select(m => gameLinkViewModelBuilder.Build(m, currentGameId))
                    .ToArray(),
                MasterGames = ownGames[GameParticipation.Authority]
                    .Select(m => gameLinkViewModelBuilder.Build(m, currentGameId))
                    .ToArray(),
                CharacterGames = ownGames[GameParticipation.Player]
                    .Select(m => gameLinkViewModelBuilder.Build(m, currentGameId))
                    .ToArray(),
                ReaderGames = ownGames[GameParticipation.Reader]
                    .Select(m => gameLinkViewModelBuilder.Build(m, currentGameId))
                    .ToArray()
            };
        }
    }
}