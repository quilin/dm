using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;
using DM.Web.API.Services.Users;
using DM.Web.Core.Authentication;
using DM.Web.Core.Authentication.Credentials;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Users
{
    /// <summary>
    /// Login
    /// </summary>
    [Route("v1/account/login")]
    public class LoginController : Controller
    {
        private readonly ILoginApiService loginApiService;

        /// <inheritdoc />
        public LoginController(
            ILoginApiService loginApiService)
        {
            this.loginApiService = loginApiService;
        }

        /// <summary>
        /// Authenticate via credentials
        /// </summary>
        /// <param name="credentials">Login credentials</param>
        /// <response code="200"></response>
        /// <response code="400">User not found or password is incorrect</response>
        /// <response code="403">User is inactive or banned</response>
        [HttpPost]
        [ProducesResponseType(typeof(Envelope<User>), 200)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        public Task<Envelope<User>> Login([FromBody] LoginCredentials credentials) =>
            loginApiService.Login(credentials, HttpContext);

        /// <summary>
        /// Logout as current user
        /// </summary>
        /// <response code="204"></response>
        /// <response code="401">User must be authenticated</response>
        [HttpDelete]
        [AuthenticationRequired]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        public Task Logout() => loginApiService.Logout(HttpContext);

        /// <summary>
        /// Logout from every device
        /// </summary>
        /// <response code="204"></response>
        /// <response code="401">User must be authenticated</response>
        [HttpDelete("all")]
        [AuthenticationRequired]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        public Task LogoutAll() => loginApiService.LogoutAll(HttpContext);
    }
}