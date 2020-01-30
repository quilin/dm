using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;
using DM.Web.API.Services.Users;
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
        /// Post user profile picture
        /// </summary>
        /// <param name="login"></param>
        /// <response code="201"></response>
        [HttpPost("{login}/uploads", Name = nameof(PostUserUpload))]
        public Task<IActionResult> PostUserUpload(string login) => throw new NotImplementedException();
    }
}