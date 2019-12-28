using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games;
using DM.Web.Core.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Gaming
{
    /// <summary>
    /// Attribute schema API controller
    /// </summary>
    [Route("v1/schemas")]
    [ApiExplorerSettings(GroupName = "Game")]
    public class AttributeSchemaController : Controller
    {
        /// <summary>
        /// Get list of game attribute schemas
        /// </summary>
        /// <response code="200"></response>
        [HttpGet(Name = nameof(GetSchemas))]
        [ProducesResponseType(typeof(ListEnvelope<AttributeSchema>), 200)]
        public Task<IActionResult> GetSchemas(GamesQuery q) => throw new NotImplementedException();

        /// <summary>
        /// Post new attribute schema
        /// </summary>
        /// <param name="schema"></param>
        /// <response code="201"></response>
        /// <response code="400">Some of schema parameters were invalid</response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to create attribute schemas</response>
        [HttpPost(Name = nameof(PostSchema))]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<Post>), 201)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        public Task<IActionResult> PostSchema([FromBody] AttributeSchema schema) => throw new NotImplementedException();
    }
}