using System;
using DM.Web.API.Dto.Common;
using DM.Web.API.Dto.Fora;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.Fora
{
    [Route("v1/topics")]
    public class TopicController : Controller
    {
        [HttpGet("{id}")]
        public Envelope<Topic> GetTopic(Guid id) => null;

        [HttpPut("{id}")]
        public Envelope<Topic> PostTopic(Guid id, [FromBody] Topic topic) => null;

        [HttpDelete("{id}")]
        public void DeleteTopic(Guid id) {}

        [HttpPost("{id}/likes")]
        public Envelope<User> PostLike(Guid id) => null;

        [HttpDelete("{id}/likes")]
        public void DeleteLike(Guid id) {}

        [HttpGet("{id}/comments")]
        public ListEnvelope<Comment> GetComments(Guid id, [FromQuery] int n = 1) => null;

        [HttpPost("{id}/comments")]
        public Envelope<Comment> PostComment(Guid id, [FromBody] Comment comment) => null;
    }
}