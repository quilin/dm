using System;
using System.Threading.Tasks;
using DM.Web.Classic.Views.GameActions;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.ViewComponents.LeftBar
{
    public class GameActions : ViewComponent
    {
        private readonly IGameActionsViewModelBuilder gameActionsViewModelBuilder;

        public GameActions(
            IGameActionsViewModelBuilder gameActionsViewModelBuilder)
        {
            this.gameActionsViewModelBuilder = gameActionsViewModelBuilder;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(Guid gameId, PageType? pageType, Guid? pageId)
        {
            var moduleActionsViewModel = await gameActionsViewModelBuilder.Build(
                gameId, pageType ?? PageType.Unknown, pageId);
            return View("~/Views/GameActions/GameActions.cshtml", moduleActionsViewModel);
        }
    }
}