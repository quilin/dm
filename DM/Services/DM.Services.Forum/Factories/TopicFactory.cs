using System;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.Forum.Dto;

namespace DM.Services.Forum.Factories
{
    /// <inheritdoc />
    public class TopicFactory : ITopicFactory
    {
        private readonly IGuidFactory guidFactory;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <inheritdoc />
        public TopicFactory(
            IGuidFactory guidFactory,
            IDateTimeProvider dateTimeProvider)
        {
            this.guidFactory = guidFactory;
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <inheritdoc />
        public ForumTopic Create(Guid forumId, Guid userId, CreateTopic createTopic)
        {
            return new ForumTopic
            {
                ForumId = forumId,
                ForumTopicId = guidFactory.Create(),
                CreateDate = dateTimeProvider.Now,
                UserId = userId,
                Title = createTopic.Title,
                Text = createTopic.Text
            };
        }
    }
}