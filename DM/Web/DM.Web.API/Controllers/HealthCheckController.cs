using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Registration.Confirmation;
using DM.Services.Core.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers
{
    /// <inheritdoc />
    public class HealthCheckController : Controller
    {
        private readonly ITemplateRenderer renderer;

        /// <inheritdoc />
        public HealthCheckController(
            ITemplateRenderer renderer)
        {
            this.renderer = renderer;
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
        public async Task<IActionResult> Test() => Ok(await renderer.Render("RegistrationLetter",
            new RegistrationConfirmationViewModel {Login = "login", ConfirmationLinkUrl = "url"}));
    }
}