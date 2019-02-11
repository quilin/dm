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
        private readonly IUserProvider userProvider;
        private readonly IAccessPolicyConverter accessPolicyConverter;
        private readonly IForumRepository forumRepository;
        private readonly ITopicRepository topicRepository;

        public ForumService(
            IUserProvider userProvider,
            IAccessPolicyConverter accessPolicyConverter,
            IForumRepository forumRepository,
            ITopicRepository topicRepository)
        {
            this.userProvider = userProvider;
            this.accessPolicyConverter = accessPolicyConverter;
            this.forumRepository = forumRepository;
            this.topicRepository = topicRepository;
        }

        public Task<IEnumerable<ForaListItem>> GetForaList()
        {
            var user = userProvider.Current;
            var accessPolicy = accessPolicyConverter.Convert(user.Role);
            return forumRepository.SelectFora(user.UserId, accessPolicy);
        }

        public Task<ForaListItem> GetForum(string forumTitle)
        {
            var user = userProvider.Current;
            var accessPolicy = accessPolicyConverter.Convert(user.Role);
            return forumRepository.GetForum(forumTitle, accessPolicy, user.UserId);
        }

        public async Task<(IEnumerable<TopicsListItem> topics, PagingData paging)> GetTopicsList(
            string forumTitle, int entityNumber)
        {
            var user = userProvider.Current;
            var accessPolicy = accessPolicyConverter.Convert(user.Role);
            var forum = await forumRepository.GetForum(forumTitle, accessPolicy);
            if (forum == null) throw new HttpException(HttpStatusCode.NotFound);

            var pageSize = userProvider.CurrentSettings.TopicsPerPage;
            var topicsCount = await topicRepository.CountTopics(forum.Id);
            var pagingData = PagingHelper.GetPaging(topicsCount, entityNumber, pageSize);
            var topics = await topicRepository.SelectTopics(user.UserId, forum.Id, pagingData);

            return (topics, pagingData);
        }
    }
}