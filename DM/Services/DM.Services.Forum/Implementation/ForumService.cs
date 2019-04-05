using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Implementation;
using DM.Services.Common.Repositories;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.Dto;
using DM.Services.Forum.Factories;
using DM.Services.Forum.Repositories;
using DM.Services.MessageQueuing.Publish;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Comment = DM.Services.Common.Dto.Comment;

namespace DM.Services.Forum.Implementation
{
    /// <inheritdoc />
    public class ForumService : IForumService
    {
        private readonly IIdentity identity;
        private readonly IValidator<CreateTopic> createTopicValidator;
        private readonly IValidator<UpdateTopic> updateTopicValidator;
        private readonly IAccessPolicyConverter accessPolicyConverter;
        private readonly IIntentionManager intentionManager;
        private readonly ITopicFactory topicFactory;
        private readonly ILikeFactory likeFactory;
        private readonly IUnreadCountersRepository unreadCountersRepository;
        private readonly IForumRepository forumRepository;
        private readonly ITopicRepository topicRepository;
        private readonly IModeratorRepository moderatorRepository;
        private readonly ILikeRepository likeRepository;
        private readonly ICommentRepository commentRepository;
        private readonly IMemoryCache memoryCache;
        private readonly IInvokedEventPublisher invokedEventPublisher;

        /// <inheritdoc />
        public ForumService(
            IValidator<CreateTopic> createTopicValidator,
            IValidator<UpdateTopic> updateTopicValidator,
            IIdentityProvider identityProvider,
            IAccessPolicyConverter accessPolicyConverter,
            IIntentionManager intentionManager,
            ITopicFactory topicFactory,
            ILikeFactory likeFactory,
            IUnreadCountersRepository unreadCountersRepository,
            IForumRepository forumRepository,
            ITopicRepository topicRepository,
            IModeratorRepository moderatorRepository,
            ILikeRepository likeRepository,
            ICommentRepository commentRepository,
            IMemoryCache memoryCache,
            IInvokedEventPublisher invokedEventPublisher)
        {
            identity = identityProvider.Current;
            this.createTopicValidator = createTopicValidator;
            this.updateTopicValidator = updateTopicValidator;
            this.accessPolicyConverter = accessPolicyConverter;
            this.intentionManager = intentionManager;
            this.topicFactory = topicFactory;
            this.likeFactory = likeFactory;
            this.unreadCountersRepository = unreadCountersRepository;
            this.forumRepository = forumRepository;
            this.topicRepository = topicRepository;
            this.moderatorRepository = moderatorRepository;
            this.likeRepository = likeRepository;
            this.commentRepository = commentRepository;
            this.memoryCache = memoryCache;
            this.invokedEventPublisher = invokedEventPublisher;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Dto.Forum>> GetForaList()
        {
            var fora = await GetFora();
            await FillCounters(fora, f => f.Id, unreadCountersRepository.SelectByParents,
                (f, c) => f.UnreadTopicsCount = c);
            return fora;
        }

        /// <inheritdoc />
        public async Task<Dto.Forum> GetForum(string forumTitle)
        {
            var forum = await FindForum(forumTitle);
            await FillCounters(new[] {forum}, f => f.Id, unreadCountersRepository.SelectByParents,
                (f, c) => f.UnreadTopicsCount = c);
            return forum;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<GeneralUser>> GetModerators(string forumTitle)
        {
            var forum = await FindForum(forumTitle);
            return await moderatorRepository.Get(forum.Id);
        }

        /// <inheritdoc />
        public async Task<(IEnumerable<Topic> topics, PagingData paging)> GetTopicsList(
            string forumTitle, int entityNumber)
        {
            var forum = await FindForum(forumTitle);

            var pageSize = identity.Settings.TopicsPerPage;
            var topicsCount = await topicRepository.Count(forum.Id);
            var pagingData = PagingData.Create(topicsCount, entityNumber, pageSize);

            var topics = (await topicRepository.Get(forum.Id, pagingData, false)).ToArray();
            await FillCounters(topics, t => t.Id, unreadCountersRepository.SelectByEntities,
                (t, c) => t.UnreadCommentsCount = c);

            return (topics, pagingData);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Topic>> GetAttachedTopics(string forumTitle)
        {
            var forum = await FindForum(forumTitle);
            var topics = (await topicRepository.Get(forum.Id, null, true)).ToArray();
            await FillCounters(topics, t => t.Id, unreadCountersRepository.SelectByEntities,
                (t, c) => t.UnreadCommentsCount = c);
            return topics;
        }

        /// <inheritdoc />
        public async Task<Topic> GetTopic(Guid topicId)
        {
            var accessPolicy = accessPolicyConverter.Convert(identity.User.Role);
            var topic = await topicRepository.Get(topicId, accessPolicy);
            if (topic == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, "Topic not found");
            }

            return topic;
        }

        /// <inheritdoc />
        public async Task<Topic> CreateTopic(CreateTopic createTopic)
        {
            await createTopicValidator.ValidateAndThrowAsync(createTopic);

            var forum = await FindForum(createTopic.ForumTitle);
            await intentionManager.ThrowIfForbidden(ForumIntention.CreateTopic, forum);

            var topic = await topicRepository.Create(topicFactory.Create(forum.Id, createTopic));

            await Task.WhenAll(
                invokedEventPublisher.Publish(EventType.NewTopic, topic.Id),
                unreadCountersRepository.Create(topic.Id, forum.Id, UnreadEntryType.Message));

            return topic;
        }

        /// <inheritdoc />
        public async Task<Topic> UpdateTopic(UpdateTopic updateTopic)
        {
            await updateTopicValidator.ValidateAndThrowAsync(updateTopic);

            var oldTopic = await GetTopic(updateTopic.TopicId);

            var topicMovesToAnotherForum = !string.IsNullOrEmpty(updateTopic.ForumTitle) &&
                                           oldTopic.Forum.Title != updateTopic.ForumTitle;
            var topicChangesClosing = updateTopic.Closed != oldTopic.Closed;
            var topicChangesAttachment = updateTopic.Attached != oldTopic.Attached;
            var hasAdministrativeChanges = topicMovesToAnotherForum || topicChangesClosing || topicChangesAttachment;
            var textChanges = !string.IsNullOrEmpty(updateTopic.Title) && updateTopic.Title != oldTopic.Title ||
                              !string.IsNullOrEmpty(updateTopic.Text) && updateTopic.Text != oldTopic.Text;

            var changes = new UpdateBuilder<ForumTopic>();
            if (hasAdministrativeChanges)
            {
                await intentionManager.ThrowIfForbidden(ForumIntention.AdministrateTopics, oldTopic.Forum);
                changes.Field(t => t.Closed, updateTopic.Closed);
                changes.Field(t => t.Attached, updateTopic.Attached);

                if (topicMovesToAnotherForum)
                {
                    var forum = (await forumRepository.SelectFora(null))
                        .First(f => f.Title == updateTopic.ForumTitle);
                    await intentionManager.ThrowIfForbidden(ForumIntention.CreateTopic, forum);
                    changes.Field(t => t.ForumId, forum.Id);
                }
            }

            if (textChanges)
            {
                await intentionManager.ThrowIfForbidden(TopicIntention.Edit, oldTopic);
                changes.Field(t => t.Title, updateTopic.Title);
                changes.Field(t => t.Text, updateTopic.Text);
            }

            var topic = await topicRepository.Update(updateTopic.TopicId, changes);
            await invokedEventPublisher.Publish(EventType.ChangedTopic, topic.Id);

            return topic;
        }

        /// <inheritdoc />
        public async Task RemoveTopic(Guid topicId)
        {
            var topic = await GetTopic(topicId);
            await intentionManager.ThrowIfForbidden(ForumIntention.AdministrateTopics, topic.Forum);

            await topicRepository.Update(topicId, new UpdateBuilder<ForumTopic>().Field(t => t.IsRemoved, true));
            await invokedEventPublisher.Publish(EventType.DeletedTopic, topicId);
        }

        /// <inheritdoc />
        public async Task<(IEnumerable<Comment> comments, PagingData paging)> GetCommentsList(
            Guid topicId, int entityNumber)
        {
            await GetTopic(topicId);

            var pageSize = identity.Settings.CommentsPerPage;
            var commentsCount = await commentRepository.Count(topicId);
            var pagingData = PagingData.Create(commentsCount, entityNumber, pageSize);

            var comments = await commentRepository.Get(topicId, pagingData);
            if (identity.User.IsAuthenticated)
            {
                await unreadCountersRepository.Flush(identity.User.UserId, UnreadEntryType.Message, topicId);
            }

            return (comments, pagingData);
        }

        /// <inheritdoc />
        public async Task<GeneralUser> LikeTopic(Guid topicId)
        {
            var topic = await GetTopic(topicId);
            await intentionManager.ThrowIfForbidden(TopicIntention.Like, topic);

            var currentUser = identity.User;
            if (topic.Likes.Any(l => l.UserId == currentUser.UserId))
            {
                throw new HttpException(HttpStatusCode.Conflict, "User already liked this topic!");
            }

            var like = likeFactory.Create(topicId, currentUser.UserId);
            await likeRepository.Add(like);
            return currentUser;
        }

        /// <inheritdoc />
        public async Task DislikeTopic(Guid topicId)
        {
            var topic = await GetTopic(topicId);
            var currentUser = identity.User;
            if (topic.Likes.All(l => l.UserId != currentUser.UserId))
            {
                throw new HttpException(HttpStatusCode.Conflict, "User never liked this topic in the first place!");
            }

            await likeRepository.Delete(topicId, currentUser.UserId);
        }

        private async Task FillCounters<TEntity>(TEntity[] entities, Func<TEntity, Guid> getId,
            Func<Guid, UnreadEntryType, Guid[], Task<IDictionary<Guid, int>>> getCounters,
            Action<TEntity, int> setCounter)
        {
            var counters = await getCounters(
                identity.User.UserId, UnreadEntryType.Message, entities.Select(getId).ToArray());
            foreach (var entity in entities)
            {
                setCounter(entity, counters[getId(entity)]);
            }
        }

        private async Task<Dto.Forum> FindForum(string forumTitle)
        {
            var forum = (await GetFora()).FirstOrDefault(f => f.Title == forumTitle);
            if (forum == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, $"Forum {forumTitle} not found");
            }

            return forum;
        }

        private async Task<Dto.Forum[]> GetFora()
        {
            var accessPolicy = accessPolicyConverter.Convert(identity.User.Role);
            return await memoryCache.GetOrCreateAsync($"ForaList_{accessPolicy}", async _ =>
                (await forumRepository.SelectFora(accessPolicy)).ToArray());
        }
    }
}