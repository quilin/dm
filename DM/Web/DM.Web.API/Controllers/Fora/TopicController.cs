using System;
using DM.Web.API.Dto.Common;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Fora;
using DM.Web.API.Dto.Users;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.Fora
{
    [Route("v1/topics")]
    public class TopicController : Controller
    {
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Envelope<Topic>), 200)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Envelope<Topic> GetTopic(Guid id) => null;

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Envelope<Topic>), 200)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(BadRequestError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Envelope<Topic> PutTopic(Guid id, [FromBody] Topic topic) => null;

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(BadRequestError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public void DeleteTopic(Guid id) {}

        [HttpPost("{id}/likes")]
        [ProducesResponseType(typeof(Envelope<User>), 201)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        [ProducesResponseType(typeof(GeneralError), 409)]
        public Envelope<User> PostLike(Guid id) => null;

        [HttpDelete("{id}/likes")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        [ProducesResponseType(typeof(GeneralError), 409)]
        public void DeleteLike(Guid id) {}

        [HttpGet("{id}/comments")]
        [ProducesResponseType(typeof(ListEnvelope<Topic>), 200)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public ListEnvelope<Comment> GetComments(Guid id, [FromQuery] int n = 1) => null;

        [HttpPost("{id}/comments")]
        [ProducesResponseType(typeof(Envelope<Comment>), 201)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Envelope<Comment> PostComment(Guid id, [FromBody] Comment comment) => null;
    }
}