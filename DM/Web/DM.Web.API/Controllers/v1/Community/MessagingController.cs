using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
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
        [HttpGet("dialogues")]
        [AuthenticationRequired]
        [ProducesResponseType()]
        public Task<IActionResult> GetConversations([FromQuery] PagingQuery q) =>
            throw new NotImplementedException();

        [HttpGet("dialogues/visavi/{login}")]
        [AuthenticationRequired]
        public Task<IActionResult> GetVisaviConversation(string login) =>
            throw new NotImplementedException();

        [HttpGet("dialogues/{id}")]
        [AuthenticationRequired]
        public Task<IActionResult> GetConversation(Guid id) =>
            throw new NotImplementedException();

        [HttpGet("dialogues/{id}/messages")]
        [AuthenticationRequired]
        public Task<IActionResult> GetMessages(Guid id, [FromQuery] PagingQuery q) =>
            throw new NotImplementedException();

        [HttpPost("dialogues/{id}/messages")]
        [AuthenticationRequired]
        public Task<IActionResult> PostMessage(Guid id, [FromBody] int message) =>
            throw new NotImplementedException();

        [HttpGet("messages/{id}")]
        [AuthenticationRequired]
        public Task<IActionResult> DeleteMessage(Guid id) =>
            throw new NotImplementedException();
    }
}