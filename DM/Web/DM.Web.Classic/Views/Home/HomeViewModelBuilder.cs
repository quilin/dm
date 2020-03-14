using System.Linq;
using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Reviews.Reading;
using DM.Services.Core.Dto;
using DM.Services.Core.Implementation;
using DM.Services.Forum.BusinessProcesses.Topics.Reading;
using DM.Web.Classic.Views.Home.News;
using DM.Web.Classic.Views.Home.Reviews;

namespace DM.Web.Classic.Views.Home
{
    public class HomeViewModelBuilder : IHomeViewModelBuilder
    {
        private readonly ITopicReadingService topicReadingService;
        private readonly INewsViewModelBuilder newsViewModelBuilder;
        private readonly IReviewReadingService reviewReadingService;
        private readonly IRandomNumberGenerator randomNumberGenerator;
        private readonly IReviewViewModelBuilder reviewViewModelBuilder;

        public HomeViewModelBuilder(
            ITopicReadingService topicReadingService,
            INewsViewModelBuilder newsViewModelBuilder,
            IReviewReadingService reviewReadingService,
            IRandomNumberGenerator randomNumberGenerator,
            IReviewViewModelBuilder reviewViewModelBuilder)
        {
            this.topicReadingService = topicReadingService;
            this.newsViewModelBuilder = newsViewModelBuilder;
            this.reviewReadingService = reviewReadingService;
            this.randomNumberGenerator = randomNumberGenerator;
            this.reviewViewModelBuilder = reviewViewModelBuilder;
        }

        public async Task<HomeViewModel> Build()
        {
            var (_, paging) = await reviewReadingService.Get(PagingQuery.Empty, true);
            var randomReviewIndex = randomNumberGenerator.Generate(0, paging.TotalEntitiesCount);
            var (reviews, _) = await reviewReadingService.Get(new PagingQuery
            {
                Skip = randomReviewIndex, Size = 1
            }, true);
            var randomReview = reviews.Any()
                ? reviewViewModelBuilder.Build(reviews.First(), true)
                : null;

            var (news, _) = await topicReadingService.GetTopicsList("Новости проекта", new PagingQuery
            {
                Size = 3,
                Skip = 0
            });

            return new HomeViewModel
            {
                RandomReview = randomReview,
                News = news.Select(newsViewModelBuilder.Build).ToArray()
            };
        }
    }
}