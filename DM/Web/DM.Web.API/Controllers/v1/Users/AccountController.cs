using System.Threading.Tasks;
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

        /// <inheritdoc />
        public AccountController(
            IRegistrationApiService registrationApiService)
        {
            this.registrationApiService = registrationApiService;
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="registration">New user credentials information</param>
        /// <response code="201"></response>
        /// <response code="400"></response>
        [HttpPost]
        public Task Register([FromBody] Registration registration) => registrationApiService.Register(registration);
    }
}