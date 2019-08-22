using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DM.Web.API.Controllers
{
    /// <inheritdoc />
    public class HealthCheckController : Controller
    {
        private readonly ILogger<HealthCheckController> logger;

        /// <inheritdoc />
        public HealthCheckController(
            ILogger<HealthCheckController> logger)
        {
            this.logger = logger;
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
        /// <param name="level"></param>
        /// <returns></returns>
        [HttpGet("_log/{level}")]
        public IActionResult Log(LogLevel level)
        {
            logger.Log(level, "Test message");
            return Ok();
        }
    }
}