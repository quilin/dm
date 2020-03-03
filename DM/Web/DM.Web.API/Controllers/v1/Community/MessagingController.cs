using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Messaging;
using DM.Web.Core.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Community
{
    /// <inheritdoc />
    [ApiController]
    [Route("v1")]
    [ApiExplorerSettings(GroupName = "Messaging")]
    public class MessagingController : ControllerBase
    {
        /// <summary>
        /// Get user conversations
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401">User must be authenticated</response>
        [HttpGet("dialogues")]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(ListEnvelope<Conversation>), 200)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        public Task<IActionResult> GetConversations([FromQuery] PagingQuery q) =>
            throw new NotImplementedException();

        /// <summary>
        /// Get conversation with user
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="410">User not found</response>
        [HttpGet("dialogues/visavi/{login}")]
        [AuthenticationRequired]
        [ProducesResponseType(302)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public Task<IActionResult> GetVisaviConversation(string login) =>
            throw new NotImplementedException();

        /// <summary>
        /// Get single conversation
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="410">Dialogue not found</response>
        [HttpGet("dialogues/{id}")]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<Conversation>), 200)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public Task<IActionResult> GetConversation(Guid id) =>
            throw new NotImplementedException();

        /// <summary>
        /// Get single conversation messages
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="410">Dialogue not found</response>
        [HttpGet("dialogues/{id}/messages")]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(ListEnvelope<Message>), 200)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public Task<IActionResult> GetMessages(Guid id, [FromQuery] PagingQuery q) =>
            throw new NotImplementedException();

        /// <summary>
        /// Create message in conversation
        /// </summary>
        /// <response code="201"></response>
        /// <response code="400">Some message parameters were invalid</response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to create message in this conversation</response>
        /// <response code="410">Dialogue not found</response>
        [HttpPost("dialogues/{id}/messages")]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(ListEnvelope<Message>), 201)]
        [ProducesResponseType(typeof(GeneralError), 400)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public Task<IActionResult> PostMessage(Guid id, [FromBody] int message) =>
            throw new NotImplementedException();

        /// <summary>
        /// Get single message
        /// </summary>
        /// <response code="200"></response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="410">Message not found</response>
        [HttpGet("messages/{id}")]
        [AuthenticationRequired]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public Task<IActionResult> DeleteMessage(Guid id) =>
            throw new NotImplementedException();
    }
}