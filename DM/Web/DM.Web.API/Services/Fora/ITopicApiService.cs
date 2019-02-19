using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Fora;

namespace DM.Web.API.Services.Fora
{
    public interface ITopicApiService
    {
        Task<ListEnvelope<Topic>> Get(string forumId, TopicFilters filters, int entityNumber);
        Task<Envelope<Topic>> Get(Guid topicId);
        Task<Envelope<Topic>> Create(string forumId, Topic topic);
    }
}