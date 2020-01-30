using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;
using DM.Web.API.Services.Users;
using DM.Web.Core.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Community
{
    /// <inheritdoc />
    [Route("v1/users")]
    [ApiExplorerSettings(GroupName = "Community")]
    public class UserController : Controller
    {
        private readonly IUserApiService userApiService;

        /// <inheritdoc />
        public UserController(
            IUserApiService userApiService)
        {
            this.userApiService = userApiService;
        }

        /// <summary>
        /// Get list of community users
        /// </summary>
        /// <response code="200"></response>
        [HttpGet("")]
        [ProducesResponseType(typeof(ListEnvelope<User>), 200)]
        public async Task<IActionResult> GetUsers([FromQuery] UsersQuery query) =>
            Ok(await userApiService.GetUsers(query));

        /// <summary>
        /// Get certain user details
        /// </summary>
        /// <param name="login"></param>
        /// <response code="200"></response>
        /// <response code="410">User not found</response>
        [HttpGet("{login}", Name = nameof(GetUser))]
        [ProducesResponseType(typeof(Envelope<User>), 200)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public async Task<IActionResult> GetUser(string login) => Ok(await userApiService.GetUser(login));

        /// <summary>
        /// Update user details
        /// </summary>
        /// <param name="login"></param>
        /// <param name="user"></param>
        /// <response code="200"></response>
        /// <response code="400">Some parameters were incorrect</response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to modify this user</response>
        /// <response code="410">User not found</response>
        [HttpPatch("{login}", Name = nameof(PutUser))]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<User>), 200)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public async Task<IActionResult> PutUser(string login, [FromBody] User user) =>
            Ok(await userApiService.UpdateUser(login, user));

        /// <summary>
        /// Post user profile picture
        /// </summary>
        /// <param name="login"></param>
        /// <response code="201"></response>
        [HttpPost("{login}/uploads", Name = nameof(PostUserUpload))]
        public Task<IActionResult> PostUserUpload(string login) => throw new NotImplementedException();
    }
}