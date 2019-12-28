using System;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Core.Parsing;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Topics.Reading;

namespace DM.Web.Classic.Views.EditTopic
{
    public class EditTopicFormBuilder : IEditTopicFormBuilder
    {
        private readonly ITopicReadingService topicReadingService;
        private readonly IIntentionManager intentionsManager;
        private readonly IBbParserProvider bbParserProvider;

        public EditTopicFormBuilder(
            ITopicReadingService topicReadingService,
            IIntentionManager intentionsManager,
            IBbParserProvider bbParserProvider
        )
        {
            this.topicReadingService = topicReadingService;
            this.intentionsManager = intentionsManager;
            this.bbParserProvider = bbParserProvider;
        }

        public async Task<EditTopicForm> Build(Guid topicId)
        {
            var topic = await topicReadingService.GetTopic(topicId);
            intentionsManager.ThrowIfForbidden(TopicIntention.Edit, topic);

            var parser = bbParserProvider.CurrentCommon;
            return new EditTopicForm
            {
                ForumId = topic.Forum.Id,
                TopicId = topicId,
                Title = topic.Title,
                Text = parser.Parse(topic.Text).ToBb(),
                Parser = parser
            };
        }
    }
}