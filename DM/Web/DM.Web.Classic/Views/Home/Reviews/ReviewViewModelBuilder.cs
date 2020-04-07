using DM.Services.Common.Authorization;
using DM.Services.Community.BusinessProcesses.Reviews;
using DM.Services.Community.BusinessProcesses.Reviews.Reading;
using DM.Services.Core.Parsing;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Home.Reviews
{
    public class ReviewViewModelBuilder : IReviewViewModelBuilder
    {
        private readonly IIntentionManager intentionManager;
        private readonly IBbParserProvider bbParserProvider;
        private readonly IUserViewModelBuilder userViewModelBuilder;

        public ReviewViewModelBuilder(
            IIntentionManager intentionManager,
            IBbParserProvider bbParserProvider,
            IUserViewModelBuilder userViewModelBuilder
        )
        {
            this.intentionManager = intentionManager;
            this.bbParserProvider = bbParserProvider;
            this.userViewModelBuilder = userViewModelBuilder;
        }

        public ReviewViewModel Build(Review review, bool isRandom = false)
        {
            if (review == null)
            {
                return null;
            }

            return new ReviewViewModel
            {
                ReviewId = review.Id,
                Author = userViewModelBuilder.Build(review.Author),
                Text = bbParserProvider.CurrentCommon.Parse(review.Text).ToHtml(),
                CanEdit = intentionManager.IsAllowed(ReviewIntention.Edit, review),
                CanApprove = intentionManager.IsAllowed(ReviewIntention.Approve, review),
                CanRemove = intentionManager.IsAllowed(ReviewIntention.Delete, review),
                IsRandom = isRandom
            };
        }
    }
}