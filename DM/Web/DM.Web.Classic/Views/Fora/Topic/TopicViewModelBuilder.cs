using DM.Services.Core.Parsing;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Fora.Topic
{
    public class TopicViewModelBuilder : ITopicViewModelBuilder
    {
        private readonly IUserViewModelBuilder userViewModelBuilder;
        private readonly IBbParserProvider bbParserProvider;

        public TopicViewModelBuilder(
            IUserViewModelBuilder userViewModelBuilder,
            IBbParserProvider bbParserProvider
            )
        {
            this.userViewModelBuilder = userViewModelBuilder;
            this.bbParserProvider = bbParserProvider;
        }

        public TopicViewModel Build(Services.Forum.Dto.Output.Topic topic)
        {
            return new TopicViewModel
            {
                ForumTopicId = topic.Id,
                CreateDate = topic.CreateDate,
                Author = userViewModelBuilder.Build(topic.Author),
                Title = topic.Title,
                Text = bbParserProvider.CurrentCommon.Parse(topic.Text).ToText(),

                LastCommentDate = topic.LastComment?.CreateDate,
                LastCommentAuthor = topic.LastComment?.Author.Login,

                Attached = topic.Attached,
                Closed = topic.Closed,

                CommentsCount = topic.TotalCommentsCount,
                UnreadCommentsCount = topic.UnreadCommentsCount
            };
        }
    }
}