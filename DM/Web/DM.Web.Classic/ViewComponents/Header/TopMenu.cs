using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Gaming.Authorization;
using DM.Web.Classic.Views.Shared.Layout;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.ViewComponents.Header
{
    public class TopMenu : ViewComponent
    {
        private readonly IIntentionManager intentionManager;

        public TopMenu(
            IIntentionManager intentionManager)
        {
            this.intentionManager = intentionManager;
        }
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var topMenuViewModel = new TopMenuViewModel
            {
                CanCreateGame = intentionManager.IsAllowed(GameIntention.Create)
            };
            return View("~/Views/Shared/Layout/TopMenu.cshtml", topMenuViewModel);
        }
    }
}