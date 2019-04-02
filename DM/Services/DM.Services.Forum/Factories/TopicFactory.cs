using System;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.Forum.Dto;

namespace DM.Services.Forum.Factories
{
    /// <inheritdoc />
    public class TopicFactory : ITopicFactory
    {
        private readonly IGuidFactory guidFactory;
        private readonly IIdentityProvider identityProvider;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <inheritdoc />
        public TopicFactory(
            IGuidFactory guidFactory,
            IIdentityProvider identityProvider,
            IDateTimeProvider dateTimeProvider)
        {
            this.guidFactory = guidFactory;
            this.identityProvider = identityProvider;
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <inheritdoc />
        public ForumTopic Create(Guid forumId, CreateTopic createTopic)
        {
            return new ForumTopic
            {
                ForumId = forumId,
                ForumTopicId = guidFactory.Create(),
                CreateDate = dateTimeProvider.Now,
                UserId = identityProvider.Current.User.UserId,
                Title = createTopic.Title,
                Text = createTopic.Text
            };
        }
    }
}