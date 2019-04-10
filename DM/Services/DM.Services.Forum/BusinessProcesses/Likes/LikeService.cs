using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Dto;
using DM.Services.Common.Implementation;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Commentaries;
using DM.Services.Forum.BusinessProcesses.Topics;
using DM.Services.MessageQueuing.Publish;

namespace DM.Services.Forum.BusinessProcesses.Likes
{
    /// <inheritdoc />
    public class LikeService : ILikeService
    {
        private readonly ITopicReadingService topicReadingService;
        private readonly ICommentaryReadingService commentaryReadingService;
        private readonly IIntentionManager intentionManager;
        private readonly ILikeFactory likeFactory;
        private readonly ILikeRepository likeRepository;
        private readonly IInvokedEventPublisher invokedEventPublisher;
        private readonly IIdentity identity;

        /// <inheritdoc />
        public LikeService(
            ITopicReadingService topicReadingService,
            ICommentaryReadingService commentaryReadingService,
            IIntentionManager intentionManager,
            IIdentityProvider identityProvider,
            ILikeFactory likeFactory,
            ILikeRepository likeRepository,
            IInvokedEventPublisher invokedEventPublisher)
        {
            this.topicReadingService = topicReadingService;
            this.commentaryReadingService = commentaryReadingService;
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
            return await Like(topic, EventType.LikedTopic);
        }

        /// <inheritdoc />
        public async Task<GeneralUser> LikeComment(Guid commentId)
        {
            var comment = await commentaryReadingService.Get(commentId);
            await intentionManager.ThrowIfForbidden(CommentIntention.Like, comment);
            return await Like(comment, EventType.LikedForumComment);
        }

        private async Task<GeneralUser> Like(ILikable entity, EventType eventType)
        {
            var currentUser = identity.User;
            if (entity.Likes.Any(l => l.UserId == currentUser.UserId))
            {
                throw new HttpException(HttpStatusCode.Conflict,
                    $"User already liked this {entity.GetType().Name.ToLower()}");
            }

            var like = likeFactory.Create(entity.Id, currentUser.UserId);
            await likeRepository.Add(like);
            await invokedEventPublisher.Publish(eventType, entity.Id);
            return currentUser;
        }

        /// <inheritdoc />
        public async Task DislikeTopic(Guid topicId)
        {
            var topic = await topicReadingService.GetTopic(topicId);
            await intentionManager.ThrowIfForbidden(TopicIntention.Like, topic);
            await Dislike(topic);
        }

        /// <inheritdoc />
        public async Task DislikeComment(Guid commentId)
        {
            var comment = await commentaryReadingService.Get(commentId);
            await intentionManager.ThrowIfForbidden(TopicIntention.Like, comment);
            await Dislike(comment);
        }

        private async Task Dislike(ILikable entity)
        {
            var currentUser = identity.User;
            if (entity.Likes.All(l => l.UserId != currentUser.UserId))
            {
                throw new HttpException(HttpStatusCode.Conflict,
                    $"User never liked this {entity.GetType().Name.ToLower()} in the first place");
            }

            await likeRepository.Delete(entity.Id, currentUser.UserId);
        }
    }
}