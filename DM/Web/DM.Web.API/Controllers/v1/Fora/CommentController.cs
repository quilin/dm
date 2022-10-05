using System;
using System.Threading.Tasks;
using DM.Web.API.Authentication;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Shared;
using DM.Web.API.Dto.Users;
using DM.Web.API.Services.Fora;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Fora;

/// <inheritdoc />
[ApiController]
[Route("v1/forum/comments")]
[ApiExplorerSettings(GroupName = "Forum")]
public class CommentController : ControllerBase
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
    [HttpGet("{id}", Name = nameof(GetForumComment))]
    [ProducesResponseType(typeof(Envelope<Comment>), 200)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> GetForumComment(Guid id) => Ok(await commentApiService.Get(id));

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
    [HttpPatch("{id}", Name = nameof(PutForumComment))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(Envelope<Comment>), 200)]
    [ProducesResponseType(typeof(BadRequestError), 400)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> PutForumComment(Guid id, [FromBody] Comment comment) =>
        Ok(await commentApiService.Update(id, comment));

    /// <summary>
    /// Delete comment
    /// </summary>
    /// <param name="id"></param>
    /// <response code="204"></response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="403">User is not allowed to change this comment</response>
    /// <response code="410">Comment not found</response>
    [HttpDelete("{id}", Name = nameof(DeleteForumComment))]
    [AuthenticationRequired]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> DeleteForumComment(Guid id)
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
    [HttpPost("{id}/likes", Name = nameof(PostForumCommentLike))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(Envelope<User>), 201)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 409)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> PostForumCommentLike(Guid id)
    {
        var result = await likeApiService.LikeComment(id);
        return CreatedAtRoute(nameof(GetForumComment), new {id}, result);
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
    [HttpDelete("{id}/likes", Name = nameof(DeleteForumCommentLike))]
    [AuthenticationRequired]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 409)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> DeleteForumCommentLike(Guid id)
    {
        await likeApiService.DislikeComment(id);
        return NoContent();
    }
}