using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Implementation;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Topics;
using DM.Services.MessageQueuing.Publish;

namespace DM.Services.Forum.BusinessProcesses.Likes
{
    /// <inheritdoc />
    public class LikeService : ILikeService
    {
        private readonly ITopicReadingService topicReadingService;
        private readonly IIntentionManager intentionManager;
        private readonly ILikeFactory likeFactory;
        private readonly ILikeRepository likeRepository;
        private readonly IInvokedEventPublisher invokedEventPublisher;
        private readonly IIdentity identity;

        /// <inheritdoc />
        public LikeService(
            ITopicReadingService topicReadingService,
            IIntentionManager intentionManager,
            IIdentityProvider identityProvider,
            ILikeFactory likeFactory,
            ILikeRepository likeRepository,
            IInvokedEventPublisher invokedEventPublisher)
        {
            this.topicReadingService = topicReadingService;
            this.intentionManager = intentionManager;
            this.likeFactory = likeFactory;
            this.likeRepository = likeRepository;
            this.invokedEventPublisher = invokedEventPublisher;
            identity = identityProvider.Current;
        }
        
        /// <inheritdoc />
        public async Task<GeneralUser> LikeTopic(Guid topicId)
        {
            var topic = await topicReadingService.GetTopic(topicId);
            await intentionManager.ThrowIfForbidden(TopicIntention.Like, topic);

            var currentUser = identity.User;
            if (topic.Likes.Any(l => l.UserId == currentUser.UserId))
            {
                throw new HttpException(HttpStatusCode.Conflict, "User already liked this topic!");
            }

            var like = likeFactory.Create(topicId, currentUser.UserId);
            await likeRepository.Add(like);
            await invokedEventPublisher.Publish(EventType.LikedTopic, like.LikeId);
            return currentUser;
        }

        /// <inheritdoc />
        public async Task DislikeTopic(Guid topicId)
        {
            var topic = await topicReadingService.GetTopic(topicId);
            await intentionManager.ThrowIfForbidden(TopicIntention.Like, topic);

            var currentUser = identity.User;
            if (topic.Likes.All(l => l.UserId != currentUser.UserId))
            {
                throw new HttpException(HttpStatusCode.Conflict, "User never liked this topic in the first place!");
            }

            await likeRepository.Delete(topicId, currentUser.UserId);
        }
    }
}