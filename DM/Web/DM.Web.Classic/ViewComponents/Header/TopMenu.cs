using System.Threading.Tasks;
using DM.Web.Classic.Views.Shared.Layout;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.ViewComponents.Header
{
    public class TopMenu : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.Run(() =>
            {
                var topMenuViewModel = new TopMenuViewModel();
                return View("~/Views/Shared/Layout/TopMenu.cshtml", topMenuViewModel);
            });
        }
    }
}