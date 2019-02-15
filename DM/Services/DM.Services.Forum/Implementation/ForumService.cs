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

        private async Task<ForaListItem[]> GetFora()
        {
            var accessPolicy = accessPolicyConverter.Convert(identity.User.Role);
            return (await forumRepository.SelectFora(accessPolicy)).ToArray();
        }

        public async Task<IEnumerable<ForaListItem>> GetForaList()
        {
            var fora = await GetFora();
            var counters = await unreadCountersRepository.SelectByParents(identity.User.UserId,
                UnreadEntryType.Message, fora.Select(f => f.Id).ToArray());
            foreach (var forum in fora)
            {
                forum.UnreadTopicsCount = counters[forum.Id];
            }

            return fora;
        }

        private async Task<ForaListItem> GetForumWithoutCounters(string forumTitle)
        {
            var forum = (await GetFora()).FirstOrDefault(f => f.Title == forumTitle);
            if (forum == null)
            {
                throw new HttpException(HttpStatusCode.NotFound, $"Forum {forumTitle} not found");
            }

            return forum;
        }

        public async Task<ForaListItem> GetForum(string forumTitle)
        {
            var forum = await GetForumWithoutCounters(forumTitle);
            forum.UnreadTopicsCount = (await unreadCountersRepository.SelectByParents(
                identity.User.UserId, UnreadEntryType.Message, forum.Id))[forum.Id];
            return forum;
        }

        public async Task<(IEnumerable<TopicsListItem> topics, PagingData paging)> GetTopicsList(
            string forumTitle, int entityNumber)
        {
            var forum = await GetForumWithoutCounters(forumTitle);

            var pageSize = identity.Settings.TopicsPerPage;
            var topicsCount = await topicRepository.CountTopics(forum.Id);
            var pagingData = PagingHelper.GetPaging(topicsCount, entityNumber, pageSize);
            var topics = await topicRepository.SelectTopics(identity.User.UserId, forum.Id, pagingData, false);

            return (topics, pagingData);
        }

        public async Task<IEnumerable<TopicsListItem>> GetAttachedTopics(string forumTitle)
        {
            var forum = await GetForumWithoutCounters(forumTitle);
            return await topicRepository.SelectTopics(identity.User.UserId, forum.Id, null, true);
        }
    }
}