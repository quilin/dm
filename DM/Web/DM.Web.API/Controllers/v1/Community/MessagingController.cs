using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Web.API.Authentication;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Messaging;
using DM.Web.API.Services.Community;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Community;

/// <inheritdoc />
[ApiController]
[Route("v1")]
[ApiExplorerSettings(GroupName = "Messaging")]
public class MessagingController : ControllerBase
{
    private readonly IMessagingApiService apiService;

    /// <inheritdoc />
    public MessagingController(
        IMessagingApiService apiService)
    {
        this.apiService = apiService;
    }

    /// <summary>
    /// Get user conversations
    /// </summary>
    /// <response code="200"></response>
    /// <response code="401">User must be authenticated</response>
    [HttpGet("dialogues", Name = nameof(GetConversations))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(ListEnvelope<Conversation>), 200)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    public async Task<IActionResult> GetConversations([FromQuery] PagingQuery q) =>
        Ok(await apiService.GetConversations(q));

    /// <summary>
    /// Get conversation with user
    /// </summary>
    /// <response code="302"></response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="410">User not found</response>
    [HttpGet("dialogues/visavi/{login}", Name = nameof(GetVisaviConversation))]
    [AuthenticationRequired]
    [ProducesResponseType(302)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> GetVisaviConversation(string login)
    {
        var conversation = await apiService.GetConversation(login);
        return RedirectToRoute(nameof(GetConversation), new {id = conversation.Resource.Id});
    }

    /// <summary>
    /// Get single conversation
    /// </summary>
    /// <response code="200"></response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="410">Dialogue not found</response>
    [HttpGet("dialogues/{id}", Name = nameof(GetConversation))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(Envelope<Conversation>), 200)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> GetConversation(Guid id) =>
        Ok(await apiService.GetConversation(id));

    /// <summary>
    /// Get single conversation messages
    /// </summary>
    /// <response code="200"></response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="410">Dialogue not found</response>
    [HttpGet("dialogues/{id}/messages", Name = nameof(GetMessages))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(ListEnvelope<Message>), 200)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> GetMessages(Guid id, [FromQuery] PagingQuery q) =>
        Ok(await apiService.GetMessages(id, q));

    /// <summary>
    /// Create message in conversation
    /// </summary>
    /// <response code="201"></response>
    /// <response code="400">Some message parameters were invalid</response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="403">User is not allowed to create message in this conversation</response>
    /// <response code="410">Dialogue not found</response>
    [HttpPost("dialogues/{id}/messages", Name = nameof(PostMessage))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(ListEnvelope<Message>), 201)]
    [ProducesResponseType(typeof(BadRequestError), 400)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> PostMessage(Guid id, [FromBody] Message message)
    {
        var result = await apiService.CreateMessage(id, message);
        return CreatedAtRoute(nameof(GetMessage), new {id = result.Resource.Id}, result);
    }

    /// <summary>
    /// Mark all messages in conversation as read
    /// </summary>
    /// <response code="204"></response>
    /// <response code="410">Dialogue not found</response>
    [HttpDelete("dialogues/{id}/messages/unread")]
    [AuthenticationRequired]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        await apiService.MarkAsRead(id);
        return NoContent();
    }

    /// <summary>
    /// Get message
    /// </summary>
    /// <response code="200"></response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="410">Message not found</response>
    [HttpGet("messages/{id}", Name = nameof(GetMessage))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(Envelope<Message>), 200)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> GetMessage(Guid id) => Ok(await apiService.GetMessage(id));

    /// <summary>
    /// Delete single message
    /// </summary>
    /// <response code="200"></response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="410">Message not found</response>
    [HttpDelete("messages/{id}", Name = nameof(DeleteMessage))]
    [AuthenticationRequired]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> DeleteMessage(Guid id)
    {
        await apiService.DeleteMessage(id);
        return NoContent();
    }
}