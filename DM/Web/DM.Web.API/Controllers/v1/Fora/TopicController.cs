using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Common;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Fora;
using DM.Web.API.Dto.Users;
using DM.Web.API.Services.Fora;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Fora
{
    [Route("v1/topics")]
    public class TopicController : Controller
    {
        private readonly ITopicApiService topicApiService;

        public TopicController(
            ITopicApiService topicApiService)
        {
            this.topicApiService = topicApiService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Envelope<Topic>), 200)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task<Envelope<Topic>> GetTopic(Guid id) => topicApiService.Get(id);

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Envelope<Topic>), 200)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(BadRequestError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task<Envelope<Topic>> PutTopic(Guid id, [FromBody] Topic topic) => throw new NotImplementedException();

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(BadRequestError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task DeleteTopic(Guid id) => throw new NotImplementedException();

        [HttpPost("{id}/likes")]
        [ProducesResponseType(typeof(Envelope<User>), 201)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        [ProducesResponseType(typeof(GeneralError), 409)]
        public Task<Envelope<User>> PostLike(Guid id) => throw new NotImplementedException();

        [HttpDelete("{id}/likes")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        [ProducesResponseType(typeof(GeneralError), 409)]
        public Task DeleteLike(Guid id) => throw new NotImplementedException();

        [HttpGet("{id}/comments")]
        [ProducesResponseType(typeof(ListEnvelope<Topic>), 200)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task<ListEnvelope<Comment>> GetComments(Guid id, [FromQuery] int n = 1) => throw new NotImplementedException();

        [HttpPost("{id}/comments")]
        [ProducesResponseType(typeof(Envelope<Comment>), 201)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task<Envelope<Comment>> PostComment(Guid id, [FromBody] Comment comment) => throw new NotImplementedException();
    }
}