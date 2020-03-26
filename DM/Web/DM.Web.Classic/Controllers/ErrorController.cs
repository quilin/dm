using DM.Web.Classic.Views.Error;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers
{
    public class ErrorController : Controller
    {
        private readonly IErrorViewModelBuilder errorViewModelBuilder;

        public ErrorController(
            IErrorViewModelBuilder errorViewModelBuilder
            )
        {
            this.errorViewModelBuilder = errorViewModelBuilder;
        }

        public IActionResult Index(int statusCode, string path)
        {
            var errorViewModel = errorViewModelBuilder.Build(statusCode, path);
            return View("~/Views/Error/Index.cshtml", errorViewModel);
        }
    }
}