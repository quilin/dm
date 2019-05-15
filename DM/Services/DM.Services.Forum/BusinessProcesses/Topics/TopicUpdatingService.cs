using System.Threading.Tasks;
using DM.Services.Common.Implementation;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Fora;
using DM.Services.Forum.Dto.Input;
using DM.Services.Forum.Dto.Output;
using DM.Services.MessageQueuing.Publish;
using FluentValidation;

namespace DM.Services.Forum.BusinessProcesses.Topics
{
    /// <inheritdoc />
    public class TopicUpdatingService : ITopicUpdatingService
    {
        private readonly IValidator<UpdateTopic> validator;
        private readonly ITopicReadingService topicReadingService;
        private readonly IForumReadingService forumReadingService;
        private readonly IIntentionManager intentionManager;
        private readonly ITopicRepository topicRepository;
        private readonly IInvokedEventPublisher invokedEventPublisher;

        /// <inheritdoc />
        public TopicUpdatingService(
            IValidator<UpdateTopic> validator,
            ITopicReadingService topicReadingService,
            IForumReadingService forumReadingService,
            IIntentionManager intentionManager,
            ITopicRepository topicRepository,
            IInvokedEventPublisher invokedEventPublisher)
        {
            this.validator = validator;
            this.topicReadingService = topicReadingService;
            this.forumReadingService = forumReadingService;
            this.intentionManager = intentionManager;
            this.topicRepository = topicRepository;
            this.invokedEventPublisher = invokedEventPublisher;
        }
        
        /// <inheritdoc />
        public async Task<Topic> UpdateTopic(UpdateTopic updateTopic)
        {
            await validator.ValidateAndThrowAsync(updateTopic);
            var oldTopic = await topicReadingService.GetTopic(updateTopic.TopicId);

            var changes = new UpdateBuilder<ForumTopic>(updateTopic.TopicId);
            if (await intentionManager.IsAllowed(ForumIntention.AdministrateTopics, oldTopic.Forum))
            {
                changes
                    .Field(t => t.Closed, updateTopic.Closed)
                    .Field(t => t.Attached, updateTopic.Attached);

                if (!string.IsNullOrEmpty(updateTopic.ForumTitle) &&
                    oldTopic.Forum.Title != updateTopic.ForumTitle)
                {
                    var forum = await forumReadingService.GetForum(updateTopic.ForumTitle, false);
                    await intentionManager.ThrowIfForbidden(ForumIntention.CreateTopic, forum);
                    changes.Field(t => t.ForumId, forum.Id);
                }
            }

            if (await intentionManager.IsAllowed(TopicIntention.Edit, oldTopic))
            {
                changes
                    .Field(t => t.Title, updateTopic.Title)
                    .Field(t => t.Text, updateTopic.Text);
            }

            var topic = await topicRepository.Update(changes);
            await invokedEventPublisher.Publish(EventType.ChangedTopic, topic.Id);

            return topic;
        }
    }
}