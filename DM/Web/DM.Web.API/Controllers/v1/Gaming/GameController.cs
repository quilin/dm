using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games;
using DM.Web.API.Dto.Shared;
using DM.Web.API.Dto.Users;
using DM.Web.API.Services.Gaming;
using DM.Web.Core.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Gaming
{
    /// <summary>
    /// Game API controller
    /// </summary>
    [Route("v1/games")]
    [ApiExplorerSettings(GroupName = "Game")]
    public class GameController : Controller
    {
        private readonly IGameApiService gameApiService;
        private readonly ICommentApiService commentApiService;
        private readonly IReaderApiService readerApiService;
        private readonly ICharacterApiService characterApiService;

        /// <inheritdoc />
        public GameController(
            IGameApiService gameApiService,
            ICommentApiService commentApiService,
            IReaderApiService readerApiService,
            ICharacterApiService characterApiService)
        {
            this.gameApiService = gameApiService;
            this.commentApiService = commentApiService;
            this.readerApiService = readerApiService;
            this.characterApiService = characterApiService;
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
        [ProducesResponseType(typeof(Envelope<Game>), 200)]
        public async Task<IActionResult> GetGame(Guid id) => Ok(await gameApiService.Get(id));

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
            var result = await gameApiService.Create(game);
            return CreatedAtRoute(nameof(GetGame), new {id = result.Resource.Id}, result);
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
        [HttpPut("{id}", Name = nameof(PutGame))]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<Game>), 201)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public async Task<IActionResult> PutGame(Guid id, [FromBody] Game game) =>
            Ok(await gameApiService.Update(id, game));

        /// <summary>
        /// Delete certain game
        /// </summary>
        /// <response code="204"></response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to remove the game</response>
        /// <response code="410">Game not found</response>
        [HttpDelete("{id}", Name = nameof(DeleteGame))]
        [AuthenticationRequired]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public async Task<IActionResult> DeleteGame(Guid id)
        {
            await gameApiService.Delete(id);
            return NoContent();
        }

        /// <summary>
        /// Get list of comments
        /// </summary>
        /// <param name="id"></param>
        /// <param name="q"></param>
        /// <response code="200"></response>
        /// <response code="410">Game not found</response>
        [HttpGet("{id}/comments", Name = nameof(GetGameComments))]
        [ProducesResponseType(typeof(ListEnvelope<Comment>), 200)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public async Task<IActionResult> GetGameComments(Guid id, [FromQuery] PagingQuery q) =>
            Ok(await commentApiService.Get(id, q));

        /// <summary>
        /// Post new game
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comment"></param>
        /// <response code="201"></response>
        /// <response code="400">Some of comment properties were invalid</response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not authorized to create a comment in this game</response>
        /// <response code="410">Game not found</response>
        [HttpPost("{id}/comments", Name = nameof(PostGameComment))]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<Comment>), 201)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public async Task<IActionResult> PostGameComment(Guid id, [FromBody] Comment comment)
        {
            var result = await commentApiService.Create(id, comment);
            return CreatedAtRoute(nameof(CommentController.GetGameComment), new {id = result.Resource.Id}, result);
        }

        /// <summary>
        /// Mark all game comments as read
        /// </summary>
        /// <param name="id">Game id</param>
        /// <response code="204"></response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="401">User is not authorized to read comments in this game</response>
        /// <response code="410">Game not found</response>
        [HttpDelete("{id}/comments/unread", Name = nameof(ReadGameComments))]
        [AuthenticationRequired]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public async Task<IActionResult> ReadGameComments(Guid id)
        {
            await commentApiService.MarkAsRead(id);
            return NoContent();
        }

        /// <summary>
        /// Get list of game readers
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200"></response>
        /// <response code="410">Game not found</response>
        [HttpGet("{id}/readers", Name = nameof(GetReaders))]
        [ProducesResponseType(typeof(ListEnvelope<User>), 200)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public async Task<IActionResult> GetReaders(Guid id) => Ok(await readerApiService.Get(id));

        /// <summary>
        /// Subscribe to the game as a reader
        /// </summary>
        /// <param name="id"></param>
        /// <response code="201"></response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not authorized to subscribe to this game</response>
        /// <response code="409">User is already subscribed to this game</response>
        /// <response code="410">Game not found</response>
        [HttpPost("{id}/readers", Name = nameof(PostReader))]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<User>), 201)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 409)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public async Task<IActionResult> PostReader(Guid id) =>
            CreatedAtRoute(nameof(GetGame), new {id}, await readerApiService.Subscribe(id));

        /// <summary>
        /// Unsubscribe from the game as a reader
        /// </summary>
        /// <param name="id"></param>
        /// <response code="201"></response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not authorized to unsubscribe from this game</response>
        /// <response code="409">User is not subscribed to this game</response>
        /// <response code="410">Game not found</response>
        [HttpDelete("{id}/readers", Name = nameof(DeleteReader))]
        [AuthenticationRequired]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 409)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public async Task<IActionResult> DeleteReader(Guid id)
        {
            await readerApiService.Unsubscribe(id);
            return NoContent();
        }

        /// <summary>
        /// Get list of game characters
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200"></response>
        /// <response code="410">Game not found</response>
        [HttpGet("{id}/characters", Name = nameof(GetCharacters))]
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
        [HttpPost("{id}/characters", Name = nameof(PostCharacter))]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<Character>), 201)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public async Task<IActionResult> PostCharacter(Guid id, Character character)
        {
            var result = await characterApiService.Create(id, character);
            return CreatedAtRoute(nameof(CharacterController.GetCharacter), new {id = result.Resource.Id}, result);
        }

        /// <summary>
        /// Get list of game rooms
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200"></response>
        /// <response code="410">Game not found</response>
        [HttpGet("{id}/rooms", Name = nameof(GetRooms))]
        [ProducesResponseType(typeof(ListEnvelope<Room>), 200)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public Task<IActionResult> GetRooms(Guid id) => throw new NotImplementedException();

        /// <summary>
        /// Post new character
        /// </summary>
        /// <param name="id"></param>
        /// <param name="room"></param>
        /// <response code="201"></response>
        /// <response code="400">Some of room properties were invalid</response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not authorized to create a room in this game</response>
        /// <response code="410">Game not found</response>
        [HttpPost("{id}/rooms", Name = nameof(PostRoom))]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<Room>), 201)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public Task<IActionResult> PostRoom(Guid id, Room room) => throw new NotImplementedException();
    }
}