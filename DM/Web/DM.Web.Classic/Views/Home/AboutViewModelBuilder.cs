using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Community.Authorization;
using DM.Services.Community.BusinessProcesses.Reviews.Reading;
using DM.Services.Core.Dto;
using DM.Web.Classic.Views.Home.CreateReview;
using DM.Web.Classic.Views.Home.Reviews;

namespace DM.Web.Classic.Views.Home
{
    public class AboutViewModelBuilder : IAboutViewModelBuilder
    {
        private readonly ICreateReviewFormBuilder createReviewFormBuilder;
        private readonly IReviewReadingService reviewService;
        private readonly IIntentionManager intentionManager;
        private readonly IReviewViewModelBuilder reviewViewModelBuilder;

        public AboutViewModelBuilder(
            ICreateReviewFormBuilder createReviewFormBuilder,
            IReviewReadingService reviewService,
            IIntentionManager intentionManager,
            IReviewViewModelBuilder reviewViewModelBuilder)
        {
            this.createReviewFormBuilder = createReviewFormBuilder;
            this.reviewService = reviewService;
            this.intentionManager = intentionManager;
            this.reviewViewModelBuilder = reviewViewModelBuilder;
        }

        public Task<AboutViewModel> Build()
        {
            var canCreate = intentionManager.IsAllowed(ReviewIntention.Create);
            return Task.FromResult(new AboutViewModel
            {
                CanCreate = canCreate,
                CreateReviewForm = canCreate ? createReviewFormBuilder.Build() : null,
                Reviews = GetAll().Select(r => reviewViewModelBuilder.Build(r)).ToArray()
            });
        }

        private IEnumerable<Review> GetAll()
        {
            var (_, paging) = reviewService.Get(PagingQuery.Empty, false).Result;
            if (paging.TotalEntitiesCount == 0)
            {
                yield break;
            }

            var position = 0;
            const int pageSize = 20;
            while (paging.TotalEntitiesCount > position)
            {
                var (reviews, _) =
                    reviewService.Get(new PagingQuery {Size = pageSize, Skip = position}, false).Result;
                position += pageSize;
                foreach (var review in reviews)
                {
                    yield return review;
                }
            }
        }
    }
}