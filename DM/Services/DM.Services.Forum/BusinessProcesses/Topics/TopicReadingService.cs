using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Extensions;
using DM.Services.Common.Repositories;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;
using DM.Services.Forum.BusinessProcesses.Common;
using DM.Services.Forum.BusinessProcesses.Fora;
using DM.Services.Forum.Dto;

namespace DM.Services.Forum.BusinessProcesses.Topics
{
    /// <inheritdoc />
    public class TopicReadingService : ITopicReadingService
    {
        private readonly IForumReadingService forumReadingService;
        private readonly IAccessPolicyConverter accessPolicyConverter;
        private readonly ITopicRepository topicRepository;
        private readonly IUnreadCountersRepository unreadCountersRepository;
        private readonly IIdentity identity;

        /// <inheritdoc />
        public TopicReadingService(
            IIdentityProvider identityProvider,
            IForumReadingService forumReadingService,
            IAccessPolicyConverter accessPolicyConverter,
            ITopicRepository topicRepository,
            IUnreadCountersRepository unreadCountersRepository)
        {
            identity = identityProvider.Current;
            this.forumReadingService = forumReadingService;
            this.accessPolicyConverter = accessPolicyConverter;
            this.topicRepository = topicRepository;
            this.unreadCountersRepository = unreadCountersRepository;
        }

        /// <inheritdoc />
        public async Task<(IEnumerable<Topic> topics, PagingData paging)> GetTopicsList(
            string forumTitle, int entityNumber)
        {
            var forum = await forumReadingService.GetForum(forumTitle);

            var pageSize = identity.Settings.TopicsPerPage;
            var topicsCount = await topicRepository.Count(forum.Id);
            var pagingData = PagingData.Create(topicsCount, entityNumber, pageSize);

            var topics = (await topicRepository.Get(forum.Id, pagingData, false)).ToArray();
            await unreadCountersRepository.FillEntityCounters(topics, identity.User.UserId,
                t => t.Id, t => t.UnreadCommentsCount);

            return (topics, pagingData);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Topic>> GetAttachedTopics(string forumTitle)
        {
            var forum = await forumReadingService.GetForum(forumTitle);
            var topics = (await topicRepository.Get(forum.Id, null, true)).ToArray();
            await unreadCountersRepository.FillEntityCounters(topics, identity.User.UserId,
                t => t.Id, t => t.UnreadCommentsCount);
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
    }
}