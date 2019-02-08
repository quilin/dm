using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Common;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Common
{
    [Route("v1/comments")]
    public class CommentController : Controller
    {
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Envelope<Comment>), 201)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task<Envelope<Comment>> PutComment(Guid id, [FromBody] Comment comment) =>
            throw new NotImplementedException();

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task DeleteComment(Guid id) => throw new NotImplementedException();

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
    }
}