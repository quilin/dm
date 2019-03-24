using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation;
using DM.Services.Common.Implementation;
using DM.Services.Common.Repositories;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.Dto;
using DM.Services.Forum.Factories;
using DM.Services.Forum.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Comment = DM.Services.Common.Dto.Comment;

namespace DM.Services.Forum.Implementation
{
    public class ForumService : IForumService
    {
        private readonly IIdentity identity;
        private readonly IAccessPolicyConverter accessPolicyConverter;
        private readonly IIntentionManager intentionManager;
        private readonly ITopicFactory topicFactory;
        private readonly IUnreadCountersRepository unreadCountersRepository;
        private readonly IForumRepository forumRepository;
        private readonly ITopicRepository topicRepository;
        private readonly IModeratorRepository moderatorRepository;
        private readonly ICommentRepository commentRepository;
        private readonly IMemoryCache memoryCache;

        public ForumService(
            IIdentityProvider identityProvider,
            IAccessPolicyConverter accessPolicyConverter,
            IIntentionManager intentionManager,
            ITopicFactory topicFactory,
            IUnreadCountersRepository unreadCountersRepository,
            IForumRepository forumRepository,
            ITopicRepository topicRepository,
            IModeratorRepository moderatorRepository,
            ICommentRepository commentRepository,
            IMemoryCache memoryCache)
        {
            identity = identityProvider.Current;
            this.accessPolicyConverter = accessPolicyConverter;
            this.intentionManager = intentionManager;
            this.topicFactory = topicFactory;
            this.unreadCountersRepository = unreadCountersRepository;
            this.forumRepository = forumRepository;
            this.topicRepository = topicRepository;
            this.moderatorRepository = moderatorRepository;
            this.commentRepository = commentRepository;
            this.memoryCache = memoryCache;
        }

        public async Task<IEnumerable<Dto.Forum>> GetForaList()
        {
            var fora = await GetFora();
            await FillCounters(fora, f => f.Id, unreadCountersRepository.SelectByParents,
                (f, c) => f.UnreadTopicsCount = c);
            return fora;
        }

        public async Task<Dto.Forum> GetForum(string forumTitle)
        {
            var forum = await FindForum(forumTitle);
            await FillCounters(new[] {forum}, f => f.Id, unreadCountersRepository.SelectByParents,
                (f, c) => f.UnreadTopicsCount = c);
            return forum;
        }

        public async Task<IEnumerable<GeneralUser>> GetModerators(string forumTitle)
        {
            var forum = await FindForum(forumTitle);
            return await moderatorRepository.Get(forum.Id);
        }

        public async Task<(IEnumerable<Topic> topics, PagingData paging)> GetTopicsList(
            string forumTitle, int entityNumber)
        {
            var forum = await FindForum(forumTitle);

            var pageSize = identity.Settings.TopicsPerPage;
            var topicsCount = await topicRepository.Count(forum.Id);
            var pagingData = PagingHelper.GetPaging(topicsCount, entityNumber, pageSize);

            var topics = (await topicRepository.Get(forum.Id, pagingData, false)).ToArray();
            await FillCounters(topics, t => t.Id, unreadCountersRepository.SelectByEntities,
                (t, c) => t.UnreadCommentsCount = c);

            return (topics, pagingData);
        }

        public async Task<IEnumerable<Topic>> GetAttachedTopics(string forumTitle)
        {
            var forum = await FindForum(forumTitle);
            var topics = (await topicRepository.Get(forum.Id, null, true)).ToArray();
            await FillCounters(topics, t => t.Id, unreadCountersRepository.SelectByEntities,
                (t, c) => t.UnreadCommentsCount = c);
            return topics;
        }

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

        public async Task<Topic> CreateTopic(CreateTopic createTopic)
        {
            var forum = await FindForum(createTopic.ForumTitle);
            await intentionManager.ThrowIfForbidden(ForumIntention.CreateTopic, forum);
            var forumTopic = topicFactory.Create(forum.Id, createTopic);
            return await topicRepository.Create(forumTopic);
        }

        public async Task<(IEnumerable<Comment> comments, PagingData paging)> GetCommentsList(
            Guid topicId, int entityNumber)
        {
            await GetTopic(topicId);

            var pageSize = identity.Settings.CommentsPerPage;
            var commentsCount = await commentRepository.Count(topicId);
            var pagingData = PagingHelper.GetPaging(commentsCount, entityNumber, pageSize);

            var comments = await commentRepository.Get(topicId, pagingData);
            if (identity.User.IsAuthenticated)
            {
                await unreadCountersRepository.Flush(identity.User.UserId, UnreadEntryType.Message, topicId);
            }

            return (comments, pagingData);
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