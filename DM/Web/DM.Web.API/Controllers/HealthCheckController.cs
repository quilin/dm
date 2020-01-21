using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers
{
    /// <inheritdoc />
    [ApiExplorerSettings(GroupName = "Service", IgnoreApi = true)]
    public class HealthCheckController : Controller
    {
        /// <summary>
        /// Health check
        /// </summary>
        /// <response code="200">API is up and ready</response>
        [HttpGet("_health")]
        public IActionResult Get() => Ok();
    }
}