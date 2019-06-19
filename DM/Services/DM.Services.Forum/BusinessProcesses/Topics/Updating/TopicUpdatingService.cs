using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Fora;
using DM.Services.Forum.BusinessProcesses.Topics.Reading;
using DM.Services.Forum.Dto.Input;
using DM.Services.Forum.Dto.Output;
using DM.Services.MessageQueuing.Publish;
using FluentValidation;

namespace DM.Services.Forum.BusinessProcesses.Topics.Updating
{
    /// <inheritdoc />
    public class TopicUpdatingService : ITopicUpdatingService
    {
        private readonly IValidator<UpdateTopic> validator;
        private readonly ITopicReadingService topicReadingService;
        private readonly IForumReadingService forumReadingService;
        private readonly IIntentionManager intentionManager;
        private readonly ITopicUpdatingRepository repository;
        private readonly IInvokedEventPublisher invokedEventPublisher;

        /// <inheritdoc />
        public TopicUpdatingService(
            IValidator<UpdateTopic> validator,
            ITopicReadingService topicReadingService,
            IForumReadingService forumReadingService,
            IIntentionManager intentionManager,
            ITopicUpdatingRepository repository,
            IInvokedEventPublisher invokedEventPublisher)
        {
            this.validator = validator;
            this.topicReadingService = topicReadingService;
            this.forumReadingService = forumReadingService;
            this.intentionManager = intentionManager;
            this.repository = repository;
            this.invokedEventPublisher = invokedEventPublisher;
        }

        /// <inheritdoc />
        public async Task<Topic> UpdateTopic(UpdateTopic updateTopic)
        {
            await validator.ValidateAndThrowAsync(updateTopic);
            var oldTopic = await topicReadingService.GetTopic(updateTopic.TopicId);

            await intentionManager.ThrowIfForbidden(TopicIntention.Edit, oldTopic);

            var changes = new UpdateBuilder<ForumTopic>(updateTopic.TopicId)
                .Field(t => t.Title, updateTopic.Title.Trim())
                .Field(t => t.Text, updateTopic.Text.Trim());

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

            var topic = await repository.Update(changes);
            await invokedEventPublisher.Publish(EventType.ChangedTopic, topic.Id);

            return topic;
        }
    }
}