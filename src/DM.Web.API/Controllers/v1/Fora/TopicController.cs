using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Web.API.Authentication;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Fora;
using DM.Web.API.Dto.Shared;
using DM.Web.API.Dto.Users;
using DM.Web.API.Services.Fora;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Fora;

/// <inheritdoc />
[ApiController]
[Route("v1/topics/{id:guid}")]
[ApiExplorerSettings(GroupName = "Forum")]
public class TopicController(
    ITopicApiService topicApiService,
    ILikeApiService likeApiService,
    ICommentApiService commentApiService) : ControllerBase
{
    /// <summary>
    /// Get certain topic
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200"></response>
    /// <response code="410">Topic not found</response>
    [HttpGet("", Name = nameof(GetTopic))]
    [ProducesResponseType(typeof(Envelope<Topic>), 200)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> GetTopic(Guid id) =>
        Ok(await topicApiService.Get(id, HttpContext.RequestAborted));

    /// <summary>
    /// Put topic changes
    /// </summary>
    /// <param name="id"></param>
    /// <param name="topic">Topic</param>
    /// <response code="200"></response>
    /// <response code="400">Some of the topic changed properties were invalid or passed id was not recognized</response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="403">User is not authorized to change some properties of this topic</response>
    /// <response code="410">Topic not found</response>
    [HttpPatch("", Name = nameof(PutTopic))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(Envelope<Topic>), 200)]
    [ProducesResponseType(typeof(BadRequestError), 400)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> PutTopic(Guid id, [FromBody] Topic topic) =>
        Ok(await topicApiService.Update(id, topic, HttpContext.RequestAborted));

    /// <summary>
    /// Delete certain topic
    /// </summary>
    /// <param name="id"></param>
    /// <response code="204"></response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="403">User is not allowed to remove the topic</response>
    /// <response code="410">Topic not found</response>
    [HttpDelete("", Name = nameof(DeleteTopic))]
    [AuthenticationRequired]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> DeleteTopic(Guid id)
    {
        await topicApiService.Delete(id, HttpContext.RequestAborted);
        return NoContent();
    }


    /// <summary>
    /// Post new like
    /// </summary>
    /// <param name="id"></param>
    /// <response code="201"></response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="403">User is not allowed to like the topic</response>
    /// <response code="409">User already liked this topic</response>
    /// <response code="410">Topic not found</response>
    [HttpPost("likes", Name = nameof(PostTopicLike))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(Envelope<User>), 201)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 409)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> PostTopicLike(Guid id)
    {
        var result = await likeApiService.LikeTopic(id, HttpContext.RequestAborted);
        return CreatedAtRoute(nameof(GetTopic), new { id }, result);
    }

    /// <summary>
    /// Delete like
    /// </summary>
    /// <param name="id"></param>
    /// <response code="204"></response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="403">User is not allowed to remove like from this topic</response>
    /// <response code="409">User has no like for this topic</response>
    /// <response code="410">Topic not found</response>
    [HttpDelete("likes", Name = nameof(DeleteTopicLike))]
    [AuthenticationRequired]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 409)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> DeleteTopicLike(Guid id)
    {
        await likeApiService.DislikeTopic(id, HttpContext.RequestAborted);
        return NoContent();
    }

    /// <summary>
    /// Get list of comments
    /// </summary>
    /// <param name="id"></param>
    /// <param name="q"></param>
    /// <response code="200"></response>
    /// <response code="410">Topic not found</response>
    [HttpGet("comments", Name = nameof(GetForumComments))]
    [ProducesResponseType(typeof(ListEnvelope<Comment>), 200)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> GetForumComments(Guid id, [FromQuery] PagingQuery q) =>
        Ok(await commentApiService.Get(id, q, HttpContext.RequestAborted));

    /// <summary>
    /// Post new comment
    /// </summary>
    /// <param name="id"></param>
    /// <param name="comment">Comment</param>
    /// <response code="201"></response>
    /// <response code="400">Some of comment properties were invalid</response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="403">User is not allowed to comment this topic</response>
    /// <response code="410">Topic not found</response>
    [HttpPost("comments", Name = nameof(PostForumComment))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(Envelope<Comment>), 201)]
    [ProducesResponseType(typeof(BadRequestError), 400)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> PostForumComment(Guid id, [FromBody] Comment comment)
    {
        var result = await commentApiService.Create(id, comment, HttpContext.RequestAborted);
        return CreatedAtRoute(nameof(CommentController.GetForumComment), new {id = result.Resource.Id}, result);
    }

    /// <summary>
    /// Mark all forum comments as read
    /// </summary>
    /// <param name="id">Topic id</param>
    /// <response code="204"></response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="410">Topic not found</response>
    [HttpDelete("comments/unread", Name = nameof(ReadTopicComments))]
    [AuthenticationRequired]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> ReadTopicComments(Guid id)
    {
        await commentApiService.MarkAsRead(id, HttpContext.RequestAborted);
        return NoContent();
    }
}