using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Common.Extensions;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Forum.BusinessProcesses.Common;
using DM.Services.Forum.BusinessProcesses.Fora;
using DM.Services.Forum.Dto.Output;

namespace DM.Services.Forum.BusinessProcesses.Topics.Reading;

/// <inheritdoc />
internal class TopicReadingService : ITopicReadingService
{
    private readonly IForumReadingService forumReadingService;
    private readonly IAccessPolicyConverter accessPolicyConverter;
    private readonly ITopicReadingRepository repository;
    private readonly IUnreadCountersRepository unreadCountersRepository;
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public TopicReadingService(
        IIdentityProvider identityProvider,
        IForumReadingService forumReadingService,
        IAccessPolicyConverter accessPolicyConverter,
        ITopicReadingRepository repository,
        IUnreadCountersRepository unreadCountersRepository)
    {
        this.identityProvider = identityProvider;
        this.forumReadingService = forumReadingService;
        this.accessPolicyConverter = accessPolicyConverter;
        this.repository = repository;
        this.unreadCountersRepository = unreadCountersRepository;
    }

    /// <inheritdoc />
    public async Task<(IEnumerable<Topic> topics, PagingResult paging)> GetTopicsList(
        string forumTitle, PagingQuery query)
    {
        var forum = await forumReadingService.GetForum(forumTitle);

        var totalCount = await repository.Count(forum.Id);
        var identity = identityProvider.Current;
        var pagingData = new PagingData(query, identity.Settings.Paging.TopicsPerPage, totalCount);

        var topics = (await repository.Get(forum.Id, pagingData, false)).ToArray();
        if (identity.User.IsAuthenticated)
        {
            await unreadCountersRepository.FillEntityCounters(topics, identity.User.UserId,
                t => t.Id, t => t.UnreadCommentsCount);
        }

        return (topics, pagingData.Result);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Topic>> GetAttachedTopics(string forumTitle)
    {
        var forum = await forumReadingService.GetForum(forumTitle);
        var topics = (await repository.Get(forum.Id, null, true)).ToArray();
        var identity = identityProvider.Current;
        if (identity.User.IsAuthenticated)
        {
            await unreadCountersRepository.FillEntityCounters(topics, identity.User.UserId,
                t => t.Id, t => t.UnreadCommentsCount);
        }

        return topics;
    }

    /// <inheritdoc />
    public async Task<Topic> GetTopic(Guid topicId)
    {
        var identity = identityProvider.Current;
        var accessPolicy = accessPolicyConverter.Convert(identity.User.Role);
        var topic = await repository.Get(topicId, accessPolicy);
        if (topic == null)
        {
            throw new HttpException(HttpStatusCode.Gone, "Topic not found");
        }

        if (identity.User.IsAuthenticated)
        {
            topic.UnreadCommentsCount = (await unreadCountersRepository.SelectByEntities(
                identity.User.UserId, UnreadEntryType.Message, topicId))[topicId];
        }

        return topic;
    }
}