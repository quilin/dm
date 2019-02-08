using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;
using DM.Web.API.Services.Users;
using DM.Web.Core.Authentication.Credentials;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1
{
    [Route("v1/account")]
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;

        public AccountController(
            IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost("")]
        public Task<Envelope<User>> Login([FromBody] LoginCredentials credentials) =>
            accountService.Login(credentials, HttpContext);
    }
}