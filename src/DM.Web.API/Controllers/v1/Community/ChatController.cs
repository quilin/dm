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
[Route("v1/chat")]
[ApiExplorerSettings(GroupName = "Messaging")]
public class ChatController : ControllerBase
{
    private readonly IChatApiService chatApiService;

    /// <inheritdoc />
    public ChatController(
        IChatApiService chatApiService)
    {
        this.chatApiService = chatApiService;
    }

    /// <summary>
    /// Get chat messages
    /// </summary>
    /// <response code="200"></response>
    [HttpGet(Name = nameof(GetChatMessages))]
    [ProducesResponseType(typeof(ListEnvelope<ChatMessage>), 200)]
    public async Task<IActionResult> GetChatMessages([FromQuery] PagingQuery q) =>
        Ok(await chatApiService.GetMessages(q));

    /// <summary>
    /// Create new chat message
    /// </summary>
    /// <response code="201"></response>
    /// <response code="400">Some message parameters were invalid</response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="403">User is not allowed to create chat messages</response>
    [HttpPost(Name = nameof(PostChatMessage))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(Envelope<ChatMessage>), 201)]
    [ProducesResponseType(typeof(Envelope<ChatMessage>), 400)]
    [ProducesResponseType(typeof(Envelope<ChatMessage>), 401)]
    [ProducesResponseType(typeof(Envelope<ChatMessage>), 403)]
    public async Task<IActionResult> PostChatMessage([FromBody] ChatMessage chatMessage)
    {
        var result = await chatApiService.CreateMessage(chatMessage);
        return CreatedAtRoute(nameof(GetChatMessage), new {id = result.Resource.Id}, result);
    }

    /// <summary>
    /// Get single chat message
    /// </summary>
    /// <response code="200"></response>
    /// <response code="410">Message not found</response>
    [HttpGet("{id}", Name = nameof(GetChatMessage))]
    [ProducesResponseType(typeof(Envelope<ChatMessage>), 200)]
    [ProducesResponseType(typeof(Envelope<ChatMessage>), 410)]
    public async Task<IActionResult> GetChatMessage(Guid id) =>
        Ok(await chatApiService.GetMessage(id));
}