using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Forum.Implementation;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Fora;
using Topic = DM.Services.Forum.Dto.Topic;

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
        
        public async Task<ListEnvelope<Dto.Fora.Topic>> Get(string forumId, TopicFilters filters, int entityNumber)
        {
            var (topics, paging) = filters.Attached
                ? (await forumService.GetAttachedTopics(forumId), null)
                : await forumService.GetTopicsList(forumId, entityNumber);
            return new ListEnvelope<Dto.Fora.Topic>(topics.Select(mapper.Map<Dto.Fora.Topic>),
                paging == null ? null : new Paging(paging));
        }

        public async Task<Envelope<Dto.Fora.Topic>> Get(Guid topicId)
        {
            var topic = await forumService.GetTopic(topicId);
            return new Envelope<Dto.Fora.Topic>(mapper.Map<Dto.Fora.Topic>(topic));
        }

        public async Task<Envelope<Dto.Fora.Topic>> Create(string forumId, Dto.Fora.Topic topic)
        {
            var topicToCreate = mapper.Map<Topic>(topic);
            var createdTopic = await forumService.CreateTopic(forumId, topicToCreate);
            return new Envelope<Dto.Fora.Topic>(mapper.Map<Dto.Fora.Topic>(createdTopic));
        }
    }
}