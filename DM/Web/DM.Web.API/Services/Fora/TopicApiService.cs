using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Forum.Dto;
using DM.Services.Forum.Implementation;
using DM.Web.API.Dto.Common;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Fora;
using Topic = DM.Web.API.Dto.Fora.Topic;

namespace DM.Web.API.Services.Fora
{
    public class TopicApiService : ITopicApiService
    {
        private readonly IForumService forumService;
        private readonly IMapper mapper;

        public TopicApiService(
            IForumService forumService,
            IMapper mapper)
        {
            this.forumService = forumService;
            this.mapper = mapper;
        }
        
        public async Task<ListEnvelope<Topic>> Get(string forumId, TopicFilters filters, int entityNumber)
        {
            var (topics, paging) = filters.Attached
                ? (await forumService.GetAttachedTopics(forumId), null)
                : await forumService.GetTopicsList(forumId, entityNumber);
            return new ListEnvelope<Topic>(topics.Select(mapper.Map<Topic>),
                paging == null ? null : new Paging(paging));
        }

        public async Task<Envelope<Topic>> Get(Guid topicId)
        {
            var topic = await forumService.GetTopic(topicId);
            return new Envelope<Topic>(mapper.Map<Topic>(topic));
        }

        public async Task<Envelope<Topic>> Create(string forumId, Topic topic)
        {
            var createTopic = mapper.Map<CreateTopic>(topic);
            createTopic.ForumTitle = forumId;
            var createdTopic = await forumService.CreateTopic(createTopic);
            return new Envelope<Topic>(mapper.Map<Topic>(createdTopic));
        }

        public async Task<ListEnvelope<Comment>> Get(Guid topicId, int entityNumber)
        {
            var (comments, paging) = await forumService.GetCommentsList(topicId, entityNumber);
            return new ListEnvelope<Comment>(comments.Select(mapper.Map<Comment>), new Paging(paging));
        }
    }
}