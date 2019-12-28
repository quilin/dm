using System;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Core.Parsing;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Commentaries.Reading;

namespace DM.Web.Classic.Views.GameCommentaries.EditCommentary
{
    public class GameCommentaryEditFormBuilder : IGameCommentaryEditFormBuilder
    {
        private readonly ICommentaryReadingService commentaryReadingService;
        private readonly IIntentionManager intentionManager;
        private readonly IBbParserProvider bbParserProvider;

        public GameCommentaryEditFormBuilder(
            ICommentaryReadingService commentaryReadingService,
            IIntentionManager intentionManager,
            IBbParserProvider bbParserProvider)
        {
            this.commentaryReadingService = commentaryReadingService;
            this.intentionManager = intentionManager;
            this.bbParserProvider = bbParserProvider;
        }

        public async Task<GameCommentaryEditForm> Build(Guid commentaryId)
        {
            var parser = bbParserProvider.CurrentCommon;
            var commentary = await commentaryReadingService.Get(commentaryId);
            intentionManager.ThrowIfForbidden(CommentIntention.Edit, commentary);

            return new GameCommentaryEditForm
            {
                CommentaryId = commentaryId,
                Text = parser.Parse(commentary.Text).ToBb(),
                Parser = parser
            };
        }
    }
}