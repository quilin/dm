using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.ViewComponents.RightBar
{
    public class Donate : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync() =>
            Task.FromResult((IViewComponentResult) View("~/Views/Shared/Layout/RightBar/Donate.cshtml"));
    }
}