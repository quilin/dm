using DM.Services.Common.Dto;
using DM.Services.Core.Parsing;

namespace DM.Web.Classic.Views.GameCommentaries.EditCommentary
{
    public class GameCommentaryEditFormBuilder : IGameCommentaryEditFormBuilder
    {
        private readonly IBbParserProvider bbParserProvider;

        public GameCommentaryEditFormBuilder(
            IBbParserProvider bbParserProvider)
        {
            this.bbParserProvider = bbParserProvider;
        }

        public GameCommentaryEditForm Build(Comment comment)
        {
            var parser = bbParserProvider.CurrentCommon;
            return new GameCommentaryEditForm
            {
                CommentaryId = comment.Id,
                Text = parser.Parse(comment.Text).ToBb(),
                Parser = parser
            };
        }
    }
}