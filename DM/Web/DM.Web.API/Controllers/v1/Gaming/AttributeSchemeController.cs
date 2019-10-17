using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games;
using DM.Web.API.Dto.Games.Attributes;
using DM.Web.Core.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Gaming
{
    /// <summary>
    /// Attribute scheme API controller
    /// </summary>
    public class AttributeSchemeController : Controller
    {
        /// <summary>
        /// Get list of game attribute schemes
        /// </summary>
        /// <response code="200"></response>
        [HttpGet(Name = nameof(GetSchemes))]
        [ProducesResponseType(typeof(ListEnvelope<AttributeScheme>), 200)]
        public Task<IActionResult> GetSchemes(GamesQuery q) => throw new NotImplementedException();

        /// <summary>
        /// Post new attribute scheme
        /// </summary>
        /// <param name="scheme"></param>
        /// <response code="201"></response>
        /// <response code="400">Some of scheme parameters were invalid</response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to create attribute schemes</response>
        [HttpPost("{id}/posts", Name = nameof(PostPost))]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<Post>), 201)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        public Task<IActionResult> PostPost([FromBody] AttributeScheme scheme) => throw new NotImplementedException();
    }
}