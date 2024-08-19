using System.Threading.Tasks;
using DM.Web.API.Authentication;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Fora;
using DM.Web.API.Dto.Users;
using DM.Web.API.Services.Fora;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Fora;

/// <inheritdoc />
[ApiController]
[Route("v1/fora")]
[ApiExplorerSettings(GroupName = "Forum")]
public class ForumController(
    IForumApiService forumApiService,
    ITopicApiService topicApiService,
    ICommentApiService commentApiService,
    IModeratorsApiService moderatorsApiService) : ControllerBase
{
    /// <summary>
    /// Get list of available fora
    /// </summary>
    /// <response code="200"></response>
    [HttpGet(Name = nameof(GetFora))]
    [ProducesResponseType(typeof(ListEnvelope<Forum>), 200)]
    public async Task<IActionResult> GetFora() =>
        Ok(await forumApiService.Get(HttpContext.RequestAborted));

    /// <summary>
    /// Get certain forum
    /// </summary>
    /// <param name="id">Forum id</param>
    /// <response code="200"></response>
    /// <response code="410">Forum not found</response>
    [HttpGet("{id}", Name = nameof(GetForum))]
    [ProducesResponseType(typeof(Envelope<Forum>), 200)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> GetForum(string id) =>
        Ok(await forumApiService.Get(id, HttpContext.RequestAborted));

    /// <summary>
    /// Mark all forum comments as read
    /// </summary>
    /// <param name="id">Forum id</param>
    /// <response code="204"></response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="410">Forum not found</response>
    [HttpDelete("{id}/comments/unread", Name = nameof(ReadForumComments))]
    [AuthenticationRequired]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> ReadForumComments(string id)
    {
        await commentApiService.MarkAsRead(id, HttpContext.RequestAborted);
        return NoContent();
    }

    /// <summary>
    /// Get forum moderators
    /// </summary>
    /// <param name="id">Forum id</param>
    /// <response code="200"></response>
    /// <response code="410">Forum not found</response>
    [HttpGet("{id}/moderators", Name = nameof(GetModerators))]
    [ProducesResponseType(typeof(ListEnvelope<User>), 200)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> GetModerators(string id) =>
        Ok(await moderatorsApiService.GetModerators(id, HttpContext.RequestAborted));

    /// <summary>
    /// Get list of forum topics
    /// </summary>
    /// <param name="id">Forum id</param>
    /// <param name="q">Query</param>
    /// <response code="200"></response>
    /// <response code="400">Some properties of the passed search parameters were invalid</response>
    /// <response code="410">Forum not found</response>
    [HttpGet("{id}/topics", Name = nameof(GetTopics))]
    [ProducesResponseType(typeof(ListEnvelope<Topic>), 200)]
    [ProducesResponseType(typeof(BadRequestError), 400)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> GetTopics(string id, [FromQuery] TopicsQuery q) =>
        Ok(await topicApiService.Get(id, q, HttpContext.RequestAborted));

    /// <summary>
    /// Post new topic
    /// </summary>
    /// <param name="id">Forum id</param>
    /// <param name="topic">New topic</param>
    /// <response code="201"></response>
    /// <response code="400">Some of the passed topic properties were invalid</response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="403">User is not allowed to create topics in this forum</response>
    /// <response code="410">Forum not found</response>
    [HttpPost("{id}/topics", Name = nameof(PostTopic))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(Envelope<Topic>), 201)]
    [ProducesResponseType(typeof(BadRequestError), 400)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> PostTopic(string id, [FromBody] Topic topic)
    {
        var result = await topicApiService.Create(id, topic, HttpContext.RequestAborted);
        return CreatedAtRoute(nameof(TopicController.GetTopic), new {id = result.Resource.Id}, result);
    }
}