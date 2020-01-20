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
        private readonly IRegistrationMailSender registrationMailSender;

        /// <summary>
        /// 
        /// </summary>
        public HealthCheckController(
            IRegistrationMailSender registrationMailSender)
        {
            this.registrationMailSender = registrationMailSender;
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
        [HttpGet("_render")]
        public async Task<IActionResult> Render() => Ok(await registrationMailSender.Send("test@email", "TestUser", Guid.NewGuid()));
    }
}