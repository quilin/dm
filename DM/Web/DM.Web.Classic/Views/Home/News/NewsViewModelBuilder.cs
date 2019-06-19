using DM.Services.Core.Parsing;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Home.News
{
    public class NewsViewModelBuilder : INewsViewModelBuilder
    {
        private readonly IBbParserProvider bbParserProvider;
        private readonly IUserViewModelBuilder userViewModelBuilder;

        public NewsViewModelBuilder(
            IBbParserProvider bbParserProvider,
            IUserViewModelBuilder userViewModelBuilder
        )
        {
            this.bbParserProvider = bbParserProvider;
            this.userViewModelBuilder = userViewModelBuilder;
        }

        public NewsViewModel Build(Services.Forum.Dto.Output.Topic topic)
        {
            return new NewsViewModel
            {
                TopicId = topic.Id,
                CreateDate = topic.CreateDate,
                Author = userViewModelBuilder.Build(topic.Author),
                Title = topic.Title,
                Text = bbParserProvider.CurrentInfo.Parse(topic.Text).ToHtml(),
                UnreadComments = topic.UnreadCommentsCount
            };
        }
    }
}