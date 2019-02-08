using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Fora;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1
{
    [Route("v1/fora")]
    public class ForumController : Controller
    {
        [HttpGet]
        [ProducesResponseType(typeof(ListEnvelope<Forum>), 200)]
        public Task<ListEnvelope<Forum>> GetFora() => throw new NotImplementedException();

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Envelope<Forum>), 200)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task<Envelope<Forum>> GetForum(string id) => throw new NotImplementedException();

        [HttpGet("{id}/topics")]
        [ProducesResponseType(typeof(ListEnvelope<Topic>), 200)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task<ListEnvelope<Topic>> GetTopics(string id, [FromQuery] TopicFilters f, [FromQuery] int n = 1) =>
            throw new NotImplementedException();

        [HttpPost("{id}/topics")]
        [ProducesResponseType(typeof(Envelope<Topic>), 201)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task<Envelope<Topic>> PostTopic(string id, [FromBody] Topic topic) =>
            throw new NotImplementedException();
    }
}