using System.Threading.Tasks;
using DM.Web.Classic.Views.Polls;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.ViewComponents.RightBar
{
    public class OpenPolls : ViewComponent
    {
        private readonly IPollsViewModelBuilder viewModelBuilder;

        public OpenPolls(
            IPollsViewModelBuilder viewModelBuilder)
        {
            this.viewModelBuilder = viewModelBuilder;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var viewModel = await viewModelBuilder.BuildActive();
            return View("~/Views/Shared/Layout/RightBar/OpenPolls.cshtml", viewModel);
        }
    }
}