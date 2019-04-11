using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;
using DM.Web.API.Services.Users;
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

        /// <inheritdoc />
        public AccountController(
            IRegistrationApiService registrationApiService,
            IActivationApiService activationApiService)
        {
            this.registrationApiService = registrationApiService;
            this.activationApiService = activationApiService;
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="registration">New user credentials information</param>
        /// <response code="201"></response>
        /// <response code="400"></response>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        public Task Register([FromBody] Registration registration) => registrationApiService.Register(registration);

        /// <summary>
        /// Activate registered user
        /// </summary>
        /// <param name="token">Activation token</param>
        /// <returns></returns>
        [HttpPut("activate/{token}")]
        [ProducesResponseType(typeof(Envelope<User>), 200)]
        public Task<Envelope<User>> Activate(Guid token) => activationApiService.Activate(token);
    }
}