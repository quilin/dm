using DM.Services.Core.Parsing;
using DM.Services.Forum.Dto.Output;

namespace DM.Web.Classic.Views.Shared.Commentaries
{
    public class EditCommentaryFormBuilder : IEditCommentaryFormBuilder
    {
        private readonly IBbParserProvider bbParserProvider;

        public EditCommentaryFormBuilder(
            IBbParserProvider bbParserProvider
        )
        {
            this.bbParserProvider = bbParserProvider;
        }

        public EditCommentaryForm Build(Comment comment)
        {
            var parser = bbParserProvider.CurrentCommon;
            return new EditCommentaryForm
            {
                CommentaryId = comment.Id,
                Text = parser.Parse(comment.Text).ToBb(),
                Parser = parser
            };
        }
    }
}