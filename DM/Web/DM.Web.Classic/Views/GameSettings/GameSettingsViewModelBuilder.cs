using System;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;

namespace DM.Web.Classic.Views.GameSettings
{
    public class GameSettingsViewModelBuilder : IGameSettingsViewModelBuilder
    {
        private readonly IGameReadingService gameService;
        private readonly IIntentionManager intentionManager;

        public GameSettingsViewModelBuilder(
            IGameReadingService gameService,
            IIntentionManager intentionManager)
        {
            this.gameService = gameService;
            this.intentionManager = intentionManager;
        }

        public async Task<GameSettingsViewModel> Build(Guid gameId, GameSettingsType settingsType)
        {
            var game = await gameService.GetGame(gameId);
            await intentionManager.ThrowIfForbidden(GameIntention.Edit, game);
            
            return new GameSettingsViewModel
            {
                GameId = gameId,
                GameTitle = game.Title,
                DefaultSettings = settingsType
            };
        }
    }
}