using System.Linq;
using DM.Services.Core.Dto;
using DM.Services.Forum.BusinessProcesses.Topics.Reading;
using DM.Web.Classic.Views.Home.News;

namespace DM.Web.Classic.Views.Home
{
    public class HomeViewModelBuilder : IHomeViewModelBuilder
    {
        private readonly ITopicReadingService topicReadingService;
        private readonly INewsViewModelBuilder newsViewModelBuilder;

        public HomeViewModelBuilder(
            ITopicReadingService topicReadingService,
            INewsViewModelBuilder newsViewModelBuilder)
        {
            this.topicReadingService = topicReadingService;
            this.newsViewModelBuilder = newsViewModelBuilder;
        }

        public HomeViewModel Build()
        {
            var (topics, _) = topicReadingService.GetTopicsList("Новости проекта", new PagingQuery
            {
                Size = 3,
                Skip = 0
            }).Result;

            return new HomeViewModel
            {
                News = topics.Select(newsViewModelBuilder.Build).ToArray()
            };
        }
    }
}