using System;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.GameRemove
{
    public class GameRemoveViewModelBuilder : IGameRemoveViewModelBuilder
    {
        private readonly IGameReadingService gameService;
        private readonly IIntentionManager intentionManager;
        private readonly IUserViewModelBuilder userViewModelBuilder;

        public GameRemoveViewModelBuilder(
            IGameReadingService gameService,
            IIntentionManager intentionManager,
            IUserViewModelBuilder userViewModelBuilder
        )
        {
            this.gameService = gameService;
            this.intentionManager = intentionManager;
            this.userViewModelBuilder = userViewModelBuilder;
        }

        public async Task<GameRemoveViewModel> Build(Guid gameId)
        {
            var game = await gameService.GetGame(gameId);
            return new GameRemoveViewModel
            {
                GameId = gameId,
                Master = userViewModelBuilder.Build(game.Master),
                CanRemove = intentionManager.IsAllowed(GameIntention.Delete, game).Result
            };
        }
    }
}