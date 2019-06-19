using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers
{
    public class ApiController : DmControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}