using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation;
using DM.Services.Common.Repositories;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Forum.Dto;
using DM.Services.Forum.Repositories;

namespace DM.Services.Forum.Implementation
{
    public class ForumService : IForumService
    {
        private readonly IIdentity identity;
        private readonly IAccessPolicyConverter accessPolicyConverter;
        private readonly IUnreadCountersRepository unreadCountersRepository;
        private readonly IForumRepository forumRepository;
        private readonly ITopicRepository topicRepository;

        public ForumService(
            IIdentityProvider identityProvider,
            IAccessPolicyConverter accessPolicyConverter,
            IUnreadCountersRepository unreadCountersRepository,
            IForumRepository forumRepository,
            ITopicRepository topicRepository)
        {
            identity = identityProvider.Current;
            this.accessPolicyConverter = accessPolicyConverter;
            this.unreadCountersRepository = unreadCountersRepository;
            this.forumRepository = forumRepository;
            this.topicRepository = topicRepository;
        }

        public async Task<IEnumerable<ForaListItem>> GetForaList()
        {
            var fora = await GetFora();
            await FillCounters(identity.User.UserId, fora, f => f.Id, (f, c) => f.UnreadTopicsCount = c);
            return fora;
        }

        public async Task<ForaListItem> GetForum(string forumTitle)
        {
            var forum = await FindForum(forumTitle);
            await FillCounters(identity.User.UserId, new[] {forum}, f => f.Id, (f, c) => f.UnreadTopicsCount = c);
            return forum;
        }

        public async Task<(IEnumerable<TopicsListItem> topics, PagingData paging)> GetTopicsList(
            string forumTitle, int entityNumber)
        {
            var forum = await FindForum(forumTitle);

            var pageSize = identity.Settings.TopicsPerPage;
            var topicsCount = await topicRepository.CountTopics(forum.Id);
            var pagingData = PagingHelper.GetPaging(topicsCount, entityNumber, pageSize);

            var userId = identity.User.UserId;
            var topics = (await topicRepository.SelectTopics(userId, forum.Id, pagingData, false)).ToArray();
            await FillCounters(userId, topics, t => t.Id, (t, c) => t.UnreadCommentsCount = c);

            return (topics, pagingData);
        }

        public async Task<IEnumerable<TopicsListItem>> GetAttachedTopics(string forumTitle)
        {
            var userId = identity.User.UserId;
            var forum = await FindForum(forumTitle);
            var topics = (await topicRepository.SelectTopics(userId, forum.Id, null, true)).ToArray();
            await FillCounters(userId, topics, t => t.Id, (t, c) => t.UnreadCommentsCount = c);
            return topics;
        }

        private async Task FillCounters<TEntity>(Guid userId, TEntity[] entities,
            Func<TEntity, Guid> getId, Action<TEntity, int> setCounter)
        {
            var counters = await unreadCountersRepository.SelectByEntities(
                userId, UnreadEntryType.Message, entities.Select(getId).ToArray());
            foreach (var entity in entities)
            {
                setCounter(entity, counters[getId(entity)]);
            }
        }

        private async Task<ForaListItem> FindForum(string forumTitle)
        {
            var forum = (await GetFora()).FirstOrDefault(f => f.Title == forumTitle);
            if (forum == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, $"Forum {forumTitle} not found");
            }

            return forum;
        }

        private async Task<ForaListItem[]> GetFora()
        {
            var accessPolicy = accessPolicyConverter.Convert(identity.User.Role);
            return (await forumRepository.SelectFora(accessPolicy)).ToArray();
        }
    }
}