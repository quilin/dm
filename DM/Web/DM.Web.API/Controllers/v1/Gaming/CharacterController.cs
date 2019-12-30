using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games;
using DM.Web.API.Services.Gaming;
using DM.Web.Core.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Gaming
{
    /// <summary>
    /// Game API controller
    /// </summary>
    [Route("v1/characters")]
    [ApiExplorerSettings(GroupName = "Game")]
    public class CharacterController : Controller
    {
        private readonly ICharacterApiService characterApiService;

        /// <inheritdoc />
        public CharacterController(
            ICharacterApiService characterApiService)
        {
            this.characterApiService = characterApiService;
        }

        /// <summary>
        /// Get certain character
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200"></response>
        /// <response code="410">Character not found</response>
        [HttpGet("{id}", Name = nameof(GetCharacter))]
        [ProducesResponseType(typeof(Envelope<Character>), 200)]
        public async Task<IActionResult> GetCharacter(Guid id) => Ok(await characterApiService.Get(id));

        /// <summary>
        /// Put character changes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="character"></param>
        /// <response code="200"></response>
        /// <response code="400">Some of character changed properties were invalid or passed id was not recognized</response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not authorized to change some properties of this character</response>
        /// <response code="410">Character not found</response>
        [HttpPatch("{id}", Name = nameof(PutCharacter))]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<Character>), 200)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public async Task<IActionResult> PutCharacter(Guid id, [FromBody] Character character) =>
            Ok(await characterApiService.Update(id, character));

        /// <summary>
        /// Delete certain character
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204"></response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to remove the character</response>
        /// <response code="410">Character not found</response>
        [HttpDelete("{id}", Name = nameof(DeleteCharacter))]
        [AuthenticationRequired]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public async Task<IActionResult> DeleteCharacter(Guid id)
        {
            await characterApiService.Delete(id);
            return NoContent();
        }
    }
}