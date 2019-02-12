using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;
using DM.Services.Core.Extensions;
using DM.Services.Forum.Dto;
using DM.Services.Forum.Repositories;

namespace DM.Services.Forum.Implementation
{
    public class ForumService : IForumService
    {
        private readonly IIdentityProvider identityProvider;
        private readonly IAccessPolicyConverter accessPolicyConverter;
        private readonly IForumRepository forumRepository;
        private readonly ITopicRepository topicRepository;

        public ForumService(
            IIdentityProvider identityProvider,
            IAccessPolicyConverter accessPolicyConverter,
            IForumRepository forumRepository,
            ITopicRepository topicRepository)
        {
            this.identityProvider = identityProvider;
            this.accessPolicyConverter = accessPolicyConverter;
            this.forumRepository = forumRepository;
            this.topicRepository = topicRepository;
        }

        public Task<IEnumerable<ForaListItem>> GetForaList()
        {
            var user = identityProvider.Current.User;
            var accessPolicy = accessPolicyConverter.Convert(user.Role);
            return forumRepository.SelectFora(user.UserId, accessPolicy);
        }

        public Task<ForaListItem> GetForum(string forumTitle)
        {
            var user = identityProvider.Current.User;
            var accessPolicy = accessPolicyConverter.Convert(user.Role);
            return forumRepository.GetForum(forumTitle, accessPolicy, user.UserId);
        }

        public async Task<(IEnumerable<TopicsListItem> topics, PagingData paging)> GetTopicsList(
            string forumTitle, int entityNumber)
        {
            var user = identityProvider.Current.User;
            var accessPolicy = accessPolicyConverter.Convert(user.Role);
            var forum = await forumRepository.GetForum(forumTitle, accessPolicy);
            if (forum == null) throw new HttpException(HttpStatusCode.NotFound);

            var pageSize = identityProvider.Current.Settings.TopicsPerPage;
            var topicsCount = await topicRepository.CountTopics(forum.Id);
            var pagingData = PagingHelper.GetPaging(topicsCount, entityNumber, pageSize);
            var topics = await topicRepository.SelectTopics(user.UserId, forum.Id, pagingData, false);

            return (topics, pagingData);
        }

        public async Task<IEnumerable<TopicsListItem>> GetAttachedTopics(string forumTitle)
        {
            var user = identityProvider.Current.User;
            var accessPolicy = accessPolicyConverter.Convert(user.Role);
            var forum = await forumRepository.GetForum(forumTitle, accessPolicy);
            if (forum == null) throw new HttpException(HttpStatusCode.NotFound);
            return await topicRepository.SelectTopics(user.UserId, forum.Id, new PagingData(), true);
        }
    }
}