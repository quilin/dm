using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games;
using DM.Web.Core.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Gaming
{
    /// <summary>
    /// Room API controller
    /// </summary>
    public class RoomController : Controller
    {
        /// <summary>
        /// Get certain room
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200"></response>
        /// <response code="410">Room not found</response>
        [HttpGet("{id}", Name = nameof(GetRoom))]
        [ProducesResponseType(typeof(Envelope<Room>), 200)]
        public Task<IActionResult> GetRoom(Guid id) => throw new NotImplementedException();

        /// <summary>
        /// Put room changes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="character"></param>
        /// <response code="200"></response>
        /// <response code="400">Some of room changed properties were invalid or passed id was not recognized</response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not authorized to change some properties of this room</response>
        /// <response code="410">Room not found</response>
        [HttpPut("{id}", Name = nameof(PutRoom))]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<Room>), 200)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public Task<IActionResult> PutRoom(Guid id, [FromBody] Character character) =>
            throw new NotImplementedException();

        /// <summary>
        /// Delete certain room
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204"></response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to remove the room</response>
        /// <response code="410">Room not found</response>
        [HttpDelete("{id}", Name = nameof(DeleteRoom))]
        [AuthenticationRequired]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public Task<IActionResult> DeleteRoom(Guid id) => throw new NotImplementedException();

        /// <summary>
        /// Get list of posts
        /// </summary>
        /// <param name="id"></param>
        /// <param name="q"></param>
        /// <response code="200"></response>
        /// <response code="410">Room not found</response>
        [HttpGet("{id}/posts", Name = nameof(GetPosts))]
        [ProducesResponseType(typeof(ListEnvelope<Post>), 200)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public Task<IActionResult> GetPosts(Guid id, [FromBody] PagingQuery q) => throw new NotImplementedException();

        /// <summary>
        /// Post new post
        /// </summary>
        /// <param name="id"></param>
        /// <param name="post"></param>
        /// <response code="201"></response>
        /// <response code="400">Some of post parameters were invalid</response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to create post in this room</response>
        /// <response code="410">Room not found</response>
        [HttpPost("{id}/posts", Name = nameof(PostPost))]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<Post>), 201)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public Task<IActionResult> PostPost(Guid id, [FromBody] Post post) => throw new NotImplementedException();
    }
}