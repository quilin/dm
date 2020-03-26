using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers
{
    public class ApiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}