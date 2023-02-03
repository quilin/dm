using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Web.API.Authentication;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games;
using DM.Web.API.Services.Gaming;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Gaming;

/// <inheritdoc />
[ApiController]
[Route("v1")]
[ApiExplorerSettings(GroupName = "Game")]
public class PostController : ControllerBase
{
    private readonly IPostApiService postApiService;

    /// <inheritdoc />
    public PostController(
        IPostApiService postApiService)
    {
        this.postApiService = postApiService;
    }

    /// <summary>
    /// Get list of posts
    /// </summary>
    /// <param name="id"></param>
    /// <param name="q"></param>
    /// <response code="200"></response>
    /// <response code="410">Room not found</response>
    [HttpGet("rooms/{id}/posts", Name = nameof(GetPosts))]
    [ProducesResponseType(typeof(ListEnvelope<Post>), 200)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> GetPosts(Guid id, [FromQuery] PagingQuery q) =>
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
    [HttpPost("rooms/{id}/posts", Name = nameof(PostPost))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(Envelope<Post>), 201)]
    [ProducesResponseType(typeof(BadRequestError), 400)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> PostPost(Guid id, [FromBody] Post post)
    {
        var result = await postApiService.Create(id, post);
        return CreatedAtRoute(nameof(GetPost),
            new {id = result.Resource.Id}, result);
    }

    /// <summary>
    /// Mark all room posts as read
    /// </summary>
    /// <param name="id"></param>
    /// <response code="204"></response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="410">Room not found</response>
    [HttpDelete("rooms/{id}/posts/unread", Name = nameof(MarkPostsAsRead))]
    [AuthenticationRequired]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> MarkPostsAsRead(Guid id)
    {
        await postApiService.MarkAsRead(id);
        return NoContent();
    }

    /// <summary>
    /// Get certain post
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200"></response>
    /// <response code="410">Post not found</response>
    [HttpGet("posts/{id}", Name = nameof(GetPost))]
    [ProducesResponseType(typeof(Envelope<Post>), 200)]
    public async Task<IActionResult> GetPost(Guid id) => Ok(await postApiService.Get(id));

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
    [HttpPatch("posts/{id}", Name = nameof(PutPost))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(Envelope<Post>), 200)]
    [ProducesResponseType(typeof(BadRequestError), 400)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> PutPost(Guid id, [FromBody] Post post) =>
        Ok(await postApiService.Update(id, post));

    /// <summary>
    /// Delete certain post
    /// </summary>
    /// <param name="id"></param>
    /// <response code="204"></response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="403">User is not allowed to remove the post</response>
    /// <response code="410">Post not found</response>
    [HttpDelete("posts/{id}", Name = nameof(DeletePost))]
    [AuthenticationRequired]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> DeletePost(Guid id)
    {
        await postApiService.Delete(id);
        return NoContent();
    }

    /// <summary>
    /// Get post rating votes
    /// </summary>
    /// <response code="200"></response>
    /// <response code="410">Post not found</response>
    [HttpGet("posts/{id}/votes", Name = nameof(GetPostVotes))]
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
    [HttpPost("posts/{id}/votes", Name = nameof(PostVote))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(Envelope<Vote>), 201)]
    [ProducesResponseType(typeof(BadRequestError), 400)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 409)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public Task<IActionResult> PostVote(Guid id, [FromBody] Vote vote) => throw new NotImplementedException();
}