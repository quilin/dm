using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Common;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;
using DM.Web.API.Services.Fora;
using DM.Web.Core.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Fora
{
    /// <summary>
    /// Commentaries
    /// </summary>
    [Route("v1/comments")]
    public class CommentController : Controller
    {
        private readonly ICommentApiService commentApiService;

        /// <inheritdoc />
        public CommentController(
            ICommentApiService commentApiService)
        {
            this.commentApiService = commentApiService;
        }

        /// <summary>
        /// Get comment
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200"></response>
        /// <response code="404">Some changed comment properties were invalid or passed id was not recognized</response>
        [ProducesResponseType(typeof(Envelope<Comment>), 200)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task<Envelope<Comment>> GetComment(Guid id) => commentApiService.Get(id);

        /// <summary>
        /// Update comment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comment"></param>
        /// <response code="200"></response>
        /// <response code="400">Some changed comment properties were invalid or passed id was not recognized</response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to change this comment</response>
        /// <response code="404">Comment was not found with passed id</response>
        [HttpPut("{id}")]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<Comment>), 200)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task<Envelope<Comment>> PutComment(Guid id, [FromBody] Comment comment) =>
            commentApiService.Update(id, comment);

        /// <summary>
        /// Delete comment
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204"></response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to change this comment</response>
        /// <response code="404">Comment was not found with passed id</response>
        [HttpDelete("{id}")]
        [AuthenticationRequired]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task DeleteComment(Guid id) => commentApiService.Delete(id);

        /// <summary>
        /// Post new like
        /// </summary>
        /// <param name="id"></param>
        /// <response code="201"></response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to like the comment</response>
        /// <response code="404">No available comment was found with passed id</response>
        /// <response code="409">User already liked this comment</response>
        [HttpPost("{id}/likes")]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<User>), 201)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        [ProducesResponseType(typeof(GeneralError), 409)]
        public Task<Envelope<User>> PostLike(Guid id) => throw new NotImplementedException();

        /// <summary>
        /// Delete like
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204"></response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to remove like from this comment</response>
        /// <response code="404">No available comment was found with passed id</response>
        /// <response code="409">User has no like for this comment</response>
        [HttpDelete("{id}/likes")]
        [AuthenticationRequired]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        [ProducesResponseType(typeof(GeneralError), 409)]
        public Task DeleteLike(Guid id) => throw new NotImplementedException();
    }
}