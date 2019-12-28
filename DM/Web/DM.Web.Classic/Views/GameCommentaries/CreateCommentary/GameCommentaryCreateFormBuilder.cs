using DM.Services.Common.Authorization;
using DM.Services.Core.Parsing;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.Dto.Output;

namespace DM.Web.Classic.Views.GameCommentaries.CreateCommentary
{
    public class GameCommentaryCreateFormBuilder : IGameCommentaryCreateFormBuilder
    {
        private readonly IIntentionManager intentionManager;
        private readonly IBbParserProvider bbParserProvider;

        public GameCommentaryCreateFormBuilder(
            IIntentionManager intentionManager,
            IBbParserProvider bbParserProvider
        )
        {
            this.intentionManager = intentionManager;
            this.bbParserProvider = bbParserProvider;
        }

        public GameCommentaryCreateForm Build(Game game)
        {
            if (intentionManager.IsAllowed(GameIntention.CreateComment, game))
            {
                return new GameCommentaryCreateForm
                {
                    GameId = game.Id,
                    Parser = bbParserProvider.CurrentCommon
                };
            }

            return null;
        }
    }
}