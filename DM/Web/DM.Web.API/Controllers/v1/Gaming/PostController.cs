using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games;
using DM.Web.Core.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Gaming
{
    /// <summary>
    /// Post API controller
    /// </summary>
    [Route("v1/posts")]
    [ApiExplorerSettings(GroupName = "Game")]
    public class PostController : Controller
    {
        /// <summary>
        /// Get certain post
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200"></response>
        /// <response code="410">Post not found</response>
        [HttpGet("{id}", Name = nameof(GetPost))]
        [ProducesResponseType(typeof(Envelope<Post>), 200)]
        public Task<IActionResult> GetPost(Guid id) => throw new NotImplementedException();

        /// <summary>
        /// Put post changes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="post"></param>
        /// <response code="200"></response>
        /// <response code="400">Some of post changed properties were invalid or passed id was not recognized</response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not authorized to change some properties of this post</response>
        /// <response code="410">Post not found</response>
        [HttpPut("{id}", Name = nameof(PutPost))]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<Post>), 200)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public Task<IActionResult> PutPost(Guid id, [FromBody] Post post) =>
            throw new NotImplementedException();

        /// <summary>
        /// Delete certain post
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204"></response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to remove the post</response>
        /// <response code="410">Post not found</response>
        [HttpDelete("{id}", Name = nameof(DeletePost))]
        [AuthenticationRequired]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public Task<IActionResult> DeletePost(Guid id) => throw new NotImplementedException();

        /// <summary>
        /// Get post rating votes
        /// </summary>
        /// <response code="200"></response>
        /// <response code="410">Post not found</response>
        [HttpGet("{id}/votes", Name = nameof(GetPostVotes))]
        [ProducesResponseType(typeof(ListEnvelope<Vote>), 200)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public Task<IActionResult> GetPostVotes(Guid id) => throw new NotImplementedException();
        
        /// <summary>
        /// Get post rating votes
        /// </summary>
        /// <response code="201"></response>
        /// <response code="400">Some of vote parameters were invalid</response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to vote for the post</response>
        /// <response code="409">User already voted for this post</response>
        /// <response code="410">Post not found</response>
        [HttpPost("{id}/votes", Name = nameof(PostVote))]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<Vote>), 201)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 409)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public Task<IActionResult> PostVote(Guid id, [FromBody] Vote vote) => throw new NotImplementedException();
    }
}