using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Shared;
using DM.Web.API.Dto.Users;
using DM.Web.API.Services.Gaming;
using DM.Web.Core.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Gaming
{
    /// <summary>
    /// Commentaries
    /// </summary>
    [Route("v1/games/comments")]
    [ApiExplorerSettings(GroupName = "Game")]
    public class CommentController : Controller
    {
        private readonly ICommentApiService commentApiService;
        private readonly ILikeApiService likeApiService;

        /// <inheritdoc />
        public CommentController(
            ICommentApiService commentApiService,
            ILikeApiService likeApiService)
        {
            this.commentApiService = commentApiService;
            this.likeApiService = likeApiService;
        }

        /// <summary>
        /// Get comment
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200"></response>
        /// <response code="410">Comment not found</response>
        [HttpGet("{id}", Name = nameof(GetGameComment))]
        [ProducesResponseType(typeof(Envelope<Comment>), 200)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public async Task<IActionResult> GetGameComment(Guid id) => Ok(await commentApiService.Get(id));

        /// <summary>
        /// Update comment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comment"></param>
        /// <response code="200"></response>
        /// <response code="400">Some changed comment properties were invalid or passed id was not recognized</response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to change this comment</response>
        /// <response code="410">Comment not found</response>
        [HttpPatch("{id}", Name = nameof(PutGameComment))]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<Comment>), 200)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public async Task<IActionResult> PutGameComment(Guid id, [FromBody] Comment comment) =>
            Ok(await commentApiService.Update(id, comment));

        /// <summary>
        /// Delete comment
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204"></response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to change this comment</response>
        /// <response code="410">Comment not found</response>
        [HttpDelete("{id}", Name = nameof(DeleteGameComment))]
        [AuthenticationRequired]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public async Task<IActionResult> DeleteGameComment(Guid id)
        {
            await commentApiService.Delete(id);
            return NoContent();
        }

        /// <summary>
        /// Post new like
        /// </summary>
        /// <param name="id"></param>
        /// <response code="201"></response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to like the comment</response>
        /// <response code="409">User already liked this comment</response>
        /// <response code="410">Comment not found</response>
        [HttpPost("{id}/likes", Name = nameof(PostGameCommentLike))]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<User>), 201)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 409)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public async Task<IActionResult> PostGameCommentLike(Guid id)
        {
            var result = await likeApiService.LikeComment(id);
            return CreatedAtRoute(nameof(GetGameComment), new {id}, result);
        }

        /// <summary>
        /// Delete like
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204"></response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to remove like from this comment</response>
        /// <response code="409">User has no like for this comment</response>
        /// <response code="410">Comment not found</response>
        [HttpDelete("{id}/likes", Name = nameof(DeleteGameCommentLike))]
        [AuthenticationRequired]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 409)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public async Task<IActionResult> DeleteGameCommentLike(Guid id)
        {
            await likeApiService.DislikeComment(id);
            return NoContent();
        }
    }
}