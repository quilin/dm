using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;

namespace DM.Web.Classic.Views.EditGameStatus
{
    public class EditGameStatusViewModelBuilder : IEditGameStatusViewModelBuilder
    {
        private readonly IGameReadingService gameService;
        private readonly IIntentionManager intentionManager;
        private readonly IEditGameStatusFormBuilder editGameStatusFormBuilder;

        public EditGameStatusViewModelBuilder(
            IGameReadingService gameService,
            IIntentionManager intentionManager,
            IEditGameStatusFormBuilder editGameStatusFormBuilder
        )
        {
            this.gameService = gameService;
            this.intentionManager = intentionManager;
            this.editGameStatusFormBuilder = editGameStatusFormBuilder;
        }

        public async Task<EditGameStatusViewModel> Build(Guid moduleId)
        {
            var game = await gameService.GetGame(moduleId);

            return new EditGameStatusViewModel
            {
                Form = editGameStatusFormBuilder.Build(game),
                StatusCredentials = new Dictionary<GameStatus, bool>
                {
                    {GameStatus.Draft, await intentionManager.IsAllowed(GameIntention.SetStatusDraft, game)},
                    {GameStatus.Requirement, await intentionManager.IsAllowed(GameIntention.SetStatusRequirement, game)},
                    {GameStatus.Active, await intentionManager.IsAllowed(GameIntention.SetStatusActive, game)},
                    {GameStatus.Frozen, await intentionManager.IsAllowed(GameIntention.SetStatusFrozen, game)},
                    {GameStatus.Finished, await intentionManager.IsAllowed(GameIntention.SetStatusFinished, game)},
                    {GameStatus.Closed, await intentionManager.IsAllowed(GameIntention.SetStatusClosed, game)},
                }
            };
        }
    }
}