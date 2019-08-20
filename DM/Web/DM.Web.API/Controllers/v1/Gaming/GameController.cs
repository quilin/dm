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
    [Route("v1/games")]
    public class GameController : Controller
    {
        private readonly IGameApiService gameApiService;

        /// <inheritdoc />
        public GameController(
            IGameApiService gameApiService)
        {
            this.gameApiService = gameApiService;
        }

        /// <summary>
        /// Get list of games
        /// </summary>
        /// <response code="200"></response>
        [HttpGet(Name = nameof(GetGames))]
        [ProducesResponseType(typeof(ListEnvelope<Game>), 200)]
        public async Task<IActionResult> GetGames(GamesQuery q) => Ok(await gameApiService.Get(q));

        /// <summary>
        /// Get certain game
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200"></response>
        /// <response code="410">Game not found</response>
        [HttpGet("{id}", Name = nameof(GetGame))]
        public async Task<IActionResult> GetGame(Guid id) => Ok(await gameApiService.Get(id));

        /// <summary>
        /// Post new game
        /// </summary>
        /// <param name="game">Game</param>
        /// <response code="201"></response>
        /// <response code="400">Some of game properties were invalid</response>
        /// <response code="401">User must be authenticated</response>
        [HttpPost(Name = nameof(CreateGame))]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<Game>), 201)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        public async Task<IActionResult> CreateGame([FromBody] Game game)
        {
            var result = await gameApiService.Create(game);
            return CreatedAtRoute(nameof(GetGame), new {id = result.Resource.Id}, result);
        }
    }
}