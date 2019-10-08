using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Users;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Users
{
    /// <inheritdoc />
    [Route("v1/users")]
    public class UserController : Controller
    {
        /// <summary>
        /// Get list of community users
        /// </summary>
        /// <response code="200"></response>
        [HttpGet("")]
        public Task<IActionResult> GetUsers([FromQuery] UsersQuery query) => throw new NotImplementedException();

        /// <summary>
        /// Get certain user details
        /// </summary>
        /// <param name="login"></param>
        /// <response code="200"></response>
        [HttpGet("{login}", Name = nameof(GetUser))]
        public Task<IActionResult> GetUser(string login) => throw new NotImplementedException();

        /// <summary>
        /// Post user profile picture
        /// </summary>
        /// <param name="login"></param>
        /// <response code="201"></response>
        [HttpPost("{login}/uploads", Name = nameof(PostUserUpload))]
        public Task<IActionResult> PostUserUpload(string login) => throw new NotImplementedException();
    }
}