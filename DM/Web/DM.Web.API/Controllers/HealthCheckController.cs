using System;
using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Registration.Confirmation;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers
{
    /// <inheritdoc />
    [ApiExplorerSettings(GroupName = "Service", IgnoreApi = true)]
    public class HealthCheckController : Controller
    {
        private readonly IRegistrationMailSender mailSender;

        /// <inheritdoc />
        public HealthCheckController(
            IRegistrationMailSender mailSender)
        {
            this.mailSender = mailSender;
        }
        
        /// <summary>
        /// Health check
        /// </summary>
        /// <response code="200">API is up and ready</response>
        [HttpGet("_health")]
        public IActionResult Get() => Ok();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("_test")]
        public async Task<IActionResult> Test() => Ok(await mailSender.Send("test@mail.com", "Quilin", Guid.NewGuid()));
    }
}