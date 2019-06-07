using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;
using DM.Web.API.Services.Users;
using DM.Web.Core.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Users
{
    /// <summary>
    /// Account
    /// </summary>
    [Route("v1/account")]
    public class AccountController : Controller
    {
        private readonly IRegistrationApiService registrationApiService;
        private readonly IActivationApiService activationApiService;
        private readonly ILoginApiService loginApiService;

        /// <inheritdoc />
        public AccountController(
            IRegistrationApiService registrationApiService,
            IActivationApiService activationApiService,
            ILoginApiService loginApiService)
        {
            this.registrationApiService = registrationApiService;
            this.activationApiService = activationApiService;
            this.loginApiService = loginApiService;
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="registration">New user credentials information</param>
        /// <response code="201">User has been registered and expects confirmation by e-mail</response>
        /// <response code="400">Some of registration properties were invalid</response>
        [HttpPost(Name = nameof(Register))]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        public async Task<IActionResult> Register([FromBody] Registration registration)
        {
            await registrationApiService.Register(registration);
            return CreatedAtRoute(nameof(UserController.GetUser), new {login = registration.Login}, null);
        }

        /// <summary>
        /// Activate registered user
        /// </summary>
        /// <param name="token">Activation token</param>
        /// <response code="200">User has been activated and logged in</response>
        /// <response code="400">Token is invalid</response>
        /// <response code="410">Token is expired</response>
        [HttpPut("{token}", Name = nameof(Activate))]
        [ProducesResponseType(typeof(Envelope<User>), 200)]
        [ProducesResponseType(typeof(GeneralError), 400)]
        [ProducesResponseType(typeof(GeneralError), 410)]
        public async Task<IActionResult> Activate(Guid token) => Ok(await activationApiService.Activate(token));

        /// <summary>
        /// Get current user
        /// </summary>
        /// <response code="201"></response>
        /// <response code="401">User must be authenticated</response>
        [AuthenticationRequired]
        [HttpGet("", Name = nameof(GetCurrent))]
        [ProducesResponseType(typeof(Envelope<User>), 200)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        public async Task<IActionResult> GetCurrent() => Ok(await loginApiService.GetCurrent());
    }
}