using DM.Web.Classic.Views.Home;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers
{
    public class HomeController : DmControllerBase
    {
        private readonly IHomeViewModelBuilder homeViewModelBuilder;

        public HomeController(
            IHomeViewModelBuilder homeViewModelBuilder)
        {
            this.homeViewModelBuilder = homeViewModelBuilder;
        }

        public IActionResult Index()
        {
            var homeViewModel = homeViewModelBuilder.Build();
            return View("Home", homeViewModel);
        }

        public ActionResult Rules()
        {
            return View();
        }

        public ActionResult Donate()
        {
            return View();
        }
    }
}