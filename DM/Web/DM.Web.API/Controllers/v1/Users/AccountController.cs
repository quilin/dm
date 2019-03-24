using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;
using DM.Web.API.Services.Users;
using DM.Web.Core.Authentication;
using DM.Web.Core.Authentication.Credentials;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Users
{
    [Route("v1/account")]
    public class AccountController : Controller
    {
        private readonly IAccountApiService accountApiService;

        public AccountController(
            IAccountApiService accountApiService)
        {
            this.accountApiService = accountApiService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Envelope<User>), 200)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        public Task<Envelope<User>> Login([FromBody] LoginCredentials credentials) =>
            accountApiService.Login(credentials, HttpContext);

        [HttpDelete]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(GeneralError), 401)]
        public Task Logout() => accountApiService.Logout(HttpContext);

        [HttpDelete("all")]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(GeneralError), 401)]
        public Task LogoutAll() => accountApiService.LogoutAll(HttpContext);
    }
}