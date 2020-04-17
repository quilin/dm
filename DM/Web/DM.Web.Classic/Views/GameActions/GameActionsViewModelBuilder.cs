using System;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;

namespace DM.Web.Classic.Views.GameActions
{
    public class GameActionsViewModelBuilder : IGameActionsViewModelBuilder
    {
        private readonly IGameReadingService gameReadingService;
        private readonly IIntentionManager intentionsManager;

        public GameActionsViewModelBuilder(
            IGameReadingService gameReadingService,
            IIntentionManager intentionsManager)
        {
            this.gameReadingService = gameReadingService;
            this.intentionsManager = intentionsManager;
        }

        public async Task<GameActionsViewModel> Build(Guid gameId, PageType pageType, Guid? pageId)
        {
            var game = await gameReadingService.GetGame(gameId);
            var canCreateCharacter = intentionsManager.IsAllowed(GameIntention.CreateCharacter, game);
            var canEditInfo = intentionsManager.IsAllowed(GameIntention.Edit, game);

            return new GameActionsViewModel
            {
                GameId = gameId,
                GameTitle = game.Title,
                CanReadCommentaries = intentionsManager.IsAllowed(GameIntention.ReadComments, game),
                CanCreateCharacter = canCreateCharacter,
                CanCreateNpc = canCreateCharacter && canEditInfo,
                CanObserve = intentionsManager.IsAllowed(GameIntention.Subscribe, game),
                CanStopObserving = intentionsManager.IsAllowed(GameIntention.Unsubscribe, game),
                CanChangeStatus = canEditInfo,
                CanTakeOnPremoderation = intentionsManager.IsAllowed(GameIntention.SetStatusModeration, game),
                CanEditInfo = canEditInfo,
                UnreadCommentariesCount = game.UnreadCommentsCount,
                UnreadCharactersCount = game.UnreadCharactersCount,
                PageType = pageType,
                PageId = pageId
            };
        }
    }
}