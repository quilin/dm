using DM.Services.Core.Parsing;

namespace DM.Web.Classic.Views.Home.CreateReview
{
    public class CreateReviewFormBuilder : ICreateReviewFormBuilder
    {
        private readonly IBbParserProvider bbParserProvider;

        public CreateReviewFormBuilder(
            IBbParserProvider bbParserProvider
        )
        {
            this.bbParserProvider = bbParserProvider;
        }

        public CreateReviewForm Build() => new CreateReviewForm
        {
            Parser = bbParserProvider.CurrentCommon
        };
    }
}