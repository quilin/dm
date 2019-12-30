using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games;
using DM.Web.API.Services.Gaming;
using DM.Web.API.Services.Gaming.Rooms;
using DM.Web.Core.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Gaming
{
    /// <summary>
    /// Room API controller
    /// </summary>
    [Route("v1/rooms")]
    [ApiExplorerSettings(GroupName = "Game")]
    public class RoomController : Controller
    {
        private readonly IRoomApiService roomApiService;
        private readonly IRoomClaimApiService claimApiService;
        private readonly IPendingPostApiService pendingPostApiService;
        private readonly IPostApiService postApiService;

        /// <inheritdoc />
        public RoomController(
            IRoomApiService roomApiService,
            IRoomClaimApiService claimApiService,
            IPendingPostApiService pendingPostApiService,
            IPostApiService postApiService)
        {
            this.roomApiService = roomApiService;
            this.claimApiService = claimApiService;
            this.pendingPostApiService = pendingPostApiService;
            this.postApiService = postApiService;
        }

        /// <summary>
        /// Get certain room
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200"></response>
        /// <response code="410">Room not found</response>
        [HttpGet("{id}", Name = nameof(GetRoom))]
        [ProducesResponseType(typeof(Envelope<Room>), 200)]
        public async Task<IActionResult> GetRoom(Guid id) => Ok(await roomApiService.Get(id));

        /// <summary>
        /// Put room changes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="room"></param>
        /// <response code="200"></response>
        /// <response code="400">Some of room changed properties were invalid or passed id was not recognized</response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not authorized to change some properties of this room</response>
        /// <response code="410">Room not found</response>
        [HttpPatch("{id}", Name = nameof(PutRoom))]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<Room>), 200)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public async Task<IActionResult> PutRoom(Guid id, [FromBody] Room room) =>
            Ok(await roomApiService.Update(id, room));

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
        public async Task<IActionResult> DeleteRoom(Guid id)
        {
            await roomApiService.Delete(id);
            return NoContent();
        }

        /// <summary>
        /// Post new room claim
        /// </summary>
        /// <param name="id"></param>
        /// <param name="claim"></param>
        /// <response code="201"></response>
        /// <response code="400">Some of claim parameters were invalid</response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to create claims in this room</response>
        /// <response code="409">Claim already exists</response>
        /// <response code="410">Room not found</response>
        [HttpPost("{id}/claims", Name = nameof(PostClaim))]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<RoomClaim>), 201)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 409)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public async Task<IActionResult> PostClaim(Guid id, [FromBody] RoomClaim claim)
        {
            var result = await claimApiService.Create(id, claim);
            return CreatedAtRoute(nameof(GetRoom),
                new {id}, result);
        }

        /// <summary>
        /// Update room claim
        /// </summary>
        /// <param name="id"></param>
        /// <param name="claim"></param>
        /// <response code="200"></response>
        /// <response code="400">Some of claim parameters were invalid</response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to update this claim</response>
        /// <response code="410">Claim not found</response>
        [HttpPatch("claims/{id}", Name = nameof(UpdateClaim))]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<RoomClaim>), 200)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public async Task<IActionResult> UpdateClaim(Guid id, [FromBody] RoomClaim claim) =>
            Ok(await claimApiService.Update(id, claim));

        /// <summary>
        /// Delete room claim
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204"></response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to delete this claim</response>
        /// <response code="410">Claim not found</response>
        [HttpDelete("claims/{id}", Name = nameof(DeleteClaim))]
        [AuthenticationRequired]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public async Task<IActionResult> DeleteClaim(Guid id)
        {
            await claimApiService.Delete(id);
            return NoContent();
        }

        /// <summary>
        /// Post new pending post
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pendingPost"></param>
        /// <response code="201"></response>
        /// <response code="400">Some of claim parameters were invalid</response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to create post pendings in this room</response>
        /// <response code="409">Post pending already exists</response>
        /// <response code="410">Room not found</response>
        [HttpPost("{id}/pendings", Name = nameof(CreatePendingPost))]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<PendingPost>), 201)]
        public async Task<IActionResult> CreatePendingPost(Guid id, [FromBody] PendingPost pendingPost)
        {
            var result = await pendingPostApiService.Create(id, pendingPost);
            return CreatedAtRoute(nameof(GetRoom), new {id}, result);
        }

        /// <summary>
        /// Delete pending post
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204"></response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to delete this pending post</response>
        /// <response code="410">Pending post not found</response>
        [HttpDelete("claims/{id}", Name = nameof(DeletePendingPost))]
        [AuthenticationRequired]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public async Task<IActionResult> DeletePendingPost(Guid id)
        {
            await pendingPostApiService.Delete(id);
            return NoContent();
        }

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
        public async Task<IActionResult> GetPosts(Guid id, [FromBody] PagingQuery q) =>
            Ok(await postApiService.Get(id, q));

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
        public async Task<IActionResult> PostPost(Guid id, [FromBody] Post post)
        {
            var result = await postApiService.Create(id, post);
            return CreatedAtRoute(nameof(PostController.GetPost),
                new {id = result.Resource.Id}, result);
        }

        /// <summary>
        /// Mark all room posts as read
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204"></response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="410">Room not found</response>
        [HttpDelete("{id}/posts/unread", Name = nameof(MarkPostsAsRead))]
        [AuthenticationRequired]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public async Task<IActionResult> MarkPostsAsRead(Guid id)
        {
            await postApiService.MarkAsRead(id);
            return NoContent();
        }
    }
}