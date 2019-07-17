using System;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Core.Parsing;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Topics.Reading;
using DM.Web.Classic.Views.Shared.Commentaries;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Topic
{
    public class TopicViewModelBuilder : ITopicViewModelBuilder
    {
        private readonly ITopicReadingService topicReadingService;
        private readonly ICommentariesViewModelBuilder commentariesViewModelBuilder;
        private readonly IIntentionManager intentionsManager;
        private readonly IUserViewModelBuilder userViewModelBuilder;
        private readonly ITopicActionsViewModelBuilder topicActionsViewModelBuilder;
        private readonly IBbParserProvider bbParserProvider;

        public TopicViewModelBuilder(
            ITopicReadingService topicReadingService,
            ICommentariesViewModelBuilder commentariesViewModelBuilder,
            IIntentionManager intentionsManager,
            IUserViewModelBuilder userViewModelBuilder,
            ITopicActionsViewModelBuilder topicActionsViewModelBuilder,
            IBbParserProvider bbParserProvider)
        {
            this.topicReadingService = topicReadingService;
            this.commentariesViewModelBuilder = commentariesViewModelBuilder;
            this.intentionsManager = intentionsManager;
            this.userViewModelBuilder = userViewModelBuilder;
            this.topicActionsViewModelBuilder = topicActionsViewModelBuilder;
            this.bbParserProvider = bbParserProvider;
        }

        public async Task<TopicViewModel> Build(Guid topicId, int entityNumber)
        {
            var topic = topicReadingService.GetTopic(topicId).Result;

            var commentariesViewModel = await commentariesViewModelBuilder.Build(topicId, entityNumber);
            commentariesViewModel.CanCreate = await intentionsManager.IsAllowed(TopicIntention.CreateComment, topic);

            return new TopicViewModel
            {
                TopicId = topicId,
                ForumId = topic.Forum.Id,
                ForumTitle = topic.Forum.Title,
                Title = topic.Title,
                Text = bbParserProvider.CurrentCommon.Parse(topic.Text).ToHtml(),
                CreateDate = topic.CreateDate,
                Attached = topic.Attached,
                Closed = topic.Closed,
                Author = userViewModelBuilder.Build(topic.Author),
                TopicActions = topicActionsViewModelBuilder.Build(topic),
                Commentaries = commentariesViewModel
            };
        }
    }
}