using System;
using System.Threading.Tasks;
using DM.Web.API.Authentication;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games;
using DM.Web.API.Dto.Users;
using DM.Web.API.Services.Gaming;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Gaming;

/// <inheritdoc />
[ApiController]
[Route("v1/games")]
[ApiExplorerSettings(GroupName = "Game")]
public class GameController(
    IGameApiService gameApiService,
    IReaderApiService readerApiService,
    IBlacklistApiService blacklistApiService) : ControllerBase
{
    /// <summary>
    /// Get list of games
    /// </summary>
    /// <response code="200"></response>
    [HttpGet(Name = nameof(GetGames))]
    [ProducesResponseType(typeof(ListEnvelope<Game>), 200)]
    public async Task<IActionResult> GetGames([FromQuery] GamesQuery q) =>
        Ok(await gameApiService.Get(q, HttpContext.RequestAborted));

    /// <summary>
    /// Get list of user own games
    /// </summary>
    /// <response code="200"></response>
    /// <response code="401">User must be authenticated</response>
    [HttpGet("own", Name = nameof(GetOwnGames))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(ListEnvelope<Game>), 200)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    public async Task<IActionResult> GetOwnGames() =>
        Ok(await gameApiService.GetOwn(HttpContext.RequestAborted));

    /// <summary>
    /// Get list of all game tags
    /// </summary>
    /// <response code="200"></response>
    [HttpGet("popular", Name = nameof(GetPopularGames))]
    [ProducesResponseType(typeof(ListEnvelope<Game>), 200)]
    public async Task<IActionResult> GetPopularGames() =>
        Ok(await gameApiService.GetPopular(HttpContext.RequestAborted));

    /// <summary>
    /// Get list of all game tags
    /// </summary>
    /// <response code="200"></response>
    [HttpGet("tags")]
    [ProducesResponseType(typeof(ListEnvelope<Tag>), 200)]
    public async Task<IActionResult> GetTags() =>
        Ok(await gameApiService.GetTags(HttpContext.RequestAborted));

    /// <summary>
    /// Get certain game
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200"></response>
    /// <response code="410">Game not found</response>
    [HttpGet("{id:guid}", Name = nameof(GetGame))]
    [ProducesResponseType(typeof(Envelope<Game>), 200)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> GetGame(Guid id) =>
        Ok(await gameApiService.Get(id, HttpContext.RequestAborted));

    /// <summary>
    /// Get certain game details
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200"></response>
    /// <response code="410">Game not found</response>
    [HttpGet("{id:guid}/details", Name = nameof(GetGameDetails))]
    [ProducesResponseType(typeof(Envelope<Game>), 200)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> GetGameDetails(Guid id) =>
        Ok(await gameApiService.GetDetails(id, HttpContext.RequestAborted));

    /// <summary>
    /// Post new game
    /// </summary>
    /// <param name="game">Game</param>
    /// <response code="201"></response>
    /// <response code="400">Some of game properties were invalid</response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="403">User is not authorized to create a game</response>
    /// <response code="410">Game not found</response>
    [HttpPost(Name = nameof(PostGame))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(Envelope<Game>), 201)]
    [ProducesResponseType(typeof(BadRequestError), 400)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> PostGame([FromBody] Game game)
    {
        var result = await gameApiService.Create(game, HttpContext.RequestAborted);
        return CreatedAtRoute(nameof(GetGameDetails), new {id = result.Resource.Id}, result);
    }

    /// <summary>
    /// Put game changes
    /// </summary>
    /// <param name="id">Game identifier</param>
    /// <param name="game">Game</param>
    /// <response code="201"></response>
    /// <response code="400">Some of game properties were invalid</response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="403">User is not authorized to change some properties of this game</response>
    /// <response code="410">Game not found</response>
    [HttpPatch("{id:guid}/details", Name = nameof(PutGame))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(Envelope<Game>), 201)]
    [ProducesResponseType(typeof(BadRequestError), 400)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> PutGame(Guid id, [FromBody] Game game) =>
        Ok(await gameApiService.Update(id, game, HttpContext.RequestAborted));

    /// <summary>
    /// Delete certain game
    /// </summary>
    /// <response code="204"></response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="403">User is not allowed to remove the game</response>
    /// <response code="410">Game not found</response>
    [HttpDelete("{id:guid}", Name = nameof(DeleteGame))]
    [AuthenticationRequired]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> DeleteGame(Guid id)
    {
        await gameApiService.Delete(id, HttpContext.RequestAborted);
        return NoContent();
    }

    /// <summary>
    /// Get list of game readers
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200"></response>
    /// <response code="410">Game not found</response>
    [HttpGet("{id:guid}/readers", Name = nameof(GetReaders))]
    [ProducesResponseType(typeof(ListEnvelope<User>), 200)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> GetReaders(Guid id) =>
        Ok(await readerApiService.Get(id, HttpContext.RequestAborted));

    /// <summary>
    /// Subscribe to the game as a reader
    /// </summary>
    /// <param name="id"></param>
    /// <response code="201"></response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="403">User is not authorized to subscribe to this game</response>
    /// <response code="409">User is already subscribed to this game</response>
    /// <response code="410">Game not found</response>
    [HttpPost("{id:guid}/readers", Name = nameof(PostReader))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(Envelope<User>), 201)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 409)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> PostReader(Guid id)
    {
        var result = await readerApiService.Subscribe(id, HttpContext.RequestAborted);
        return CreatedAtRoute(nameof(GetGame), new { id }, result);
    }

    /// <summary>
    /// Unsubscribe from the game as a reader
    /// </summary>
    /// <param name="id"></param>
    /// <response code="201"></response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="403">User is not authorized to unsubscribe from this game</response>
    /// <response code="409">User is not subscribed to this game</response>
    /// <response code="410">Game not found</response>
    [HttpDelete("{id:guid}/readers", Name = nameof(DeleteReader))]
    [AuthenticationRequired]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 409)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> DeleteReader(Guid id)
    {
        await readerApiService.Unsubscribe(id, HttpContext.RequestAborted);
        return NoContent();
    }

    /// <summary>
    /// Get game blacklisted users
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200"></response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="403">User is not authorized to read blacklist of this game</response>
    /// <response code="410">Game not found</response>
    [HttpGet("{id:guid}/blacklist/users", Name = nameof(GetBlacklist))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(ListEnvelope<User>), 200)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> GetBlacklist(Guid id) =>
        Ok(await blacklistApiService.Get(id, HttpContext.RequestAborted));

    /// <summary>
    /// Post new blacklisted user
    /// </summary>
    /// <param name="id"></param>
    /// <param name="user"></param>
    /// <response code="201"></response>
    /// <response code="400">Some user properties were invalid</response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="403">User is not authorized to blacklist users in this game</response>
    /// <response code="409">User is already blacklisted</response>
    /// <response code="410">Game not found</response>
    [HttpPost("{id:guid}/blacklist/users", Name = nameof(PostBlacklist))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(Envelope<User>), 201)]
    [ProducesResponseType(typeof(BadRequestError), 400)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 409)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> PostBlacklist(Guid id, [FromBody] User user)
    {
        var result = await blacklistApiService.Create(id, user, HttpContext.RequestAborted);
        return CreatedAtRoute(nameof(GetBlacklist), new {id}, result);
    }

    /// <summary>
    /// Delete blacklisted user link
    /// </summary>
    /// <param name="id"></param>
    /// <param name="login"></param>
    /// <response code="204"></response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="403">User is not authorized to un-blacklist users in this game</response>
    /// <response code="409">User is not in the blacklist</response>
    /// <response code="410">Game not found</response>
    [HttpDelete("{id:guid}/blacklist/users/{login}", Name = nameof(DeleteBlacklist))]
    [AuthenticationRequired]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 409)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> DeleteBlacklist(Guid id, string login)
    {
        await blacklistApiService.Delete(id, login, HttpContext.RequestAborted);
        return NoContent();
    }
}