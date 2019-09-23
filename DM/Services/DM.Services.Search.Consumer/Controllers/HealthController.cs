using Microsoft.AspNetCore.Mvc;

namespace DM.Services.Search.Consumer.Controllers
{
    /// <summary>
    /// Health check endpoints
    /// </summary>
    [Route("api")]
    public class HealthController : Controller
    {
        /// <summary>
        /// Tells if API is ready to process requests
        /// </summary>
        /// <returns></returns>
        [HttpGet("_ready")]
        public IActionResult Ready() => Ok();

        /// <summary>
        /// Tells if API is alive and working
        /// </summary>
        /// <returns></returns>
        [HttpGet("_alive")]
        public IActionResult Alive() => Ok();
    }
}