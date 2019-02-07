using System;
using DM.Web.API.Dto.Common;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.Common
{
    [Route("v1/comments")]
    public class CommentController : Controller
    {
        [HttpPut("{id}")]
        public Envelope<Comment> PutComment(Guid id, [FromBody] Comment comment) => null;

        [HttpDelete("{id}")]
        public void DeleteComment(Guid id) {}

        [HttpPost("{id}/likes")]
        public Envelope<User> PostLike(Guid id) => null;

        [HttpDelete("{id}/likes")]
        public void DeleteLike(Guid id) {}
    }
}