using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Forum.BusinessProcesses.Topics.Creating;
using DM.Services.Forum.BusinessProcesses.Topics.Deleting;
using DM.Services.Forum.BusinessProcesses.Topics.Reading;
using DM.Services.Forum.BusinessProcesses.Topics.Updating;
using DM.Services.Forum.Dto.Input;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Fora;
using PagingQuery = DM.Services.Core.Dto.PagingQuery;
using Topic = DM.Web.API.Dto.Fora.Topic;

namespace DM.Web.API.Services.Fora;

/// <inheritdoc />
internal class TopicApiService : ITopicApiService
{
    private readonly ITopicReadingService topicReadingService;
    private readonly ITopicCreatingService topicCreatingService;
    private readonly ITopicUpdatingService topicUpdatingService;
    private readonly ITopicDeletingService topicDeletingService;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public TopicApiService(
        ITopicReadingService topicReadingService,
        ITopicCreatingService topicCreatingService,
        ITopicUpdatingService topicUpdatingService,
        ITopicDeletingService topicDeletingService,
        IMapper mapper)
    {
        this.topicReadingService = topicReadingService;
        this.topicCreatingService = topicCreatingService;
        this.topicUpdatingService = topicUpdatingService;
        this.topicDeletingService = topicDeletingService;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ListEnvelope<Topic>> Get(string forumId, TopicsQuery query)
    {
        var (topics, paging) = query.Attached
            ? (await topicReadingService.GetAttachedTopics(forumId), null)
            : await topicReadingService.GetTopicsList(forumId, mapper.Map<PagingQuery>(query));
        return new ListEnvelope<Topic>(topics.Select(mapper.Map<Topic>),
            paging == null ? null : new Paging(paging));
    }

    /// <inheritdoc />
    public async Task<Envelope<Topic>> Get(Guid topicId)
    {
        var topic = await topicReadingService.GetTopic(topicId);
        return new Envelope<Topic>(mapper.Map<Topic>(topic));
    }

    /// <inheritdoc />
    public async Task<Envelope<Topic>> Create(string forumId, Topic topic)
    {
        var createTopic = mapper.Map<CreateTopic>(topic);
        createTopic.ForumTitle = forumId;
        var createdTopic = await topicCreatingService.CreateTopic(createTopic);
        return new Envelope<Topic>(mapper.Map<Topic>(createdTopic));
    }

    /// <inheritdoc />
    public async Task<Envelope<Topic>> Update(Guid topicId, Topic topic)
    {
        var updateTopic = mapper.Map<UpdateTopic>(topic);
        updateTopic.TopicId = topicId;
        var updatedTopic = await topicUpdatingService.UpdateTopic(updateTopic);
        return new Envelope<Topic>(mapper.Map<Topic>(updatedTopic));
    }

    /// <inheritdoc />
    public Task Delete(Guid topicId) => topicDeletingService.DeleteTopic(topicId);
}