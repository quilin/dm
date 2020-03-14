﻿using System.Threading.Tasks;
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
            IAboutViewModelBuilder aboutViewModelBuilder)
        {
            this.homeViewModelBuilder = homeViewModelBuilder;
            this.aboutViewModelBuilder = aboutViewModelBuilder;
        }

        public IActionResult Index() => View(homeViewModelBuilder.Build());

        public async Task<IActionResult> About() => View(await aboutViewModelBuilder.Build());

        public IActionResult Rules() => View();

        public IActionResult Donate() => View();

        public IActionResult Api() => View();
    }
}