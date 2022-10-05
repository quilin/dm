using System;
using System.Threading.Tasks;
using DM.Web.API.Authentication;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games;
using DM.Web.API.Services.Gaming;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Gaming;

/// <inheritdoc />
[ApiController]
[Route("v1")]
[ApiExplorerSettings(GroupName = "Game")]
public class CharacterController : ControllerBase
{
    private readonly ICharacterApiService characterApiService;

    /// <inheritdoc />
    public CharacterController(
        ICharacterApiService characterApiService)
    {
        this.characterApiService = characterApiService;
    }

    /// <summary>
    /// Get list of game characters
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200"></response>
    /// <response code="410">Game not found</response>
    [HttpGet("games/{id}/characters", Name = nameof(GetCharacters))]
    [ProducesResponseType(typeof(ListEnvelope<Character>), 200)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> GetCharacters(Guid id) => Ok(await characterApiService.GetAll(id));

    /// <summary>
    /// Post new character
    /// </summary>
    /// <param name="id"></param>
    /// <param name="character"></param>
    /// <response code="201"></response>
    /// <response code="400">Some of character properties were invalid</response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="403">User is not authorized to create a character in this game</response>
    /// <response code="410">Game not found</response>
    [HttpPost("games/{id}/characters", Name = nameof(PostCharacter))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(Envelope<Character>), 201)]
    [ProducesResponseType(typeof(BadRequestError), 400)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> PostCharacter(Guid id, [FromBody] Character character)
    {
        var result = await characterApiService.Create(id, character);
        return CreatedAtRoute(nameof(GetCharacter),
            new {id = result.Resource.Id}, result);
    }

    /// <summary>
    /// Mark all game characters as read
    /// </summary>
    /// <param name="id">Game id</param>
    /// <response code="204"></response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="401">User is not authorized to read characters in this game</response>
    /// <response code="410">Game not found</response>
    [HttpDelete("game/{id}/characters/unread", Name = nameof(ReadGameCharacters))]
    [AuthenticationRequired]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> ReadGameCharacters(Guid id)
    {
        await characterApiService.MarkAsRead(id);
        return NoContent();
    }

    /// <summary>
    /// Get certain character
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200"></response>
    /// <response code="410">Character not found</response>
    [HttpGet("characters/{id}", Name = nameof(GetCharacter))]
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
    [HttpPatch("characters/{id}", Name = nameof(PutCharacter))]
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
    [HttpDelete("characters/{id}", Name = nameof(DeleteCharacter))]
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