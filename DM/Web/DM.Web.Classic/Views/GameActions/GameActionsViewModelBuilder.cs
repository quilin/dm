using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.Dto;

namespace DM.Web.Classic.Views.GameActions
{
    public class GameActionsViewModelBuilder : IGameActionsViewModelBuilder
    {
        private readonly IGameReadingService gameService;
        private readonly IIntentionManager intentionManager;
        private readonly IIdentity identity;

        public GameActionsViewModelBuilder(
            IGameReadingService gameService,
            IIntentionManager intentionManager,
            IIdentityProvider identityProvider)
        {
            this.gameService = gameService;
            this.intentionManager = intentionManager;
            identity = identityProvider.Current;
        }

        public async Task<GameActionsViewModel> Build(Guid gameId, PageType pageType, Guid? pageId)
        {
            var game = await gameService.GetGame(gameId);
            var participation = game.Participation(identity.User.UserId);

            return new GameActionsViewModel
            {
                GameId = gameId,
                GameTitle = game.Title,
                CanReadCommentaries = intentionManager.IsAllowed(GameIntention.ReadComments, game).Result,
                CanCreateCharacter = intentionManager.IsAllowed(GameIntention.CreateCharacter, game).Result,
                CanCreateNpc = participation.HasFlag(GameParticipation.Authority),
                CanObserve = intentionManager.IsAllowed(GameIntention.Subscribe, game).Result,
                CanChangeStatus = participation.HasFlag(GameParticipation.Authority),
                CanTakeOnPremoderation = intentionManager.IsAllowed(GameIntention.SetStatusModeration, game).Result,
                CanEditInfo = intentionManager.IsAllowed(GameIntention.Edit, game).Result,
                UnreadCommentariesCount = game.UnreadCommentsCount,
                PageType = pageType,
                PageId = pageId
            };
        }
    }
}