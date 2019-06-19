using DM.Web.Classic.Views.Home;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers
{
    public class HomeController : DmControllerBase
    {
        private readonly IHomeViewModelBuilder homeViewModelBuilder;
        private readonly IAboutViewModelBuilder aboutViewModelBuilder;

        public HomeController(
            IHomeViewModelBuilder homeViewModelBuilder,
            IAboutViewModelBuilder aboutViewModelBuilder
        )
        {
            this.homeViewModelBuilder = homeViewModelBuilder;
            this.aboutViewModelBuilder = aboutViewModelBuilder;
        }

        public IActionResult Index()
        {
            var homeViewModel = homeViewModelBuilder.Build();
            return View("Home", homeViewModel);
        }

        public ActionResult About()
        {
            var aboutViewModel = aboutViewModelBuilder.Build();
            return View(aboutViewModel);
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