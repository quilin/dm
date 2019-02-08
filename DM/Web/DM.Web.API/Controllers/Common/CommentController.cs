using System;
using DM.Web.API.Dto.Common;
using DM.Web.API.Dto.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.Common
{
    [Route("v1/comments")]
    public class CommentController : Controller
    {
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Envelope<Comment>), 201)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Envelope<Comment> PutComment(Guid id, [FromBody] Comment comment) => null;

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public void DeleteComment(Guid id) {}

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
    }
}