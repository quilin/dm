using System;
using System.Threading.Tasks;
using DM.Web.Classic.Views.GameActions;
using DM.Web.Classic.Views.GameActions.GameRooms;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.ViewComponents.LeftBar
{
    public class GameRooms : ViewComponent
    {
        private readonly IGameRoomsViewModelBuilder gameRoomsViewModelBuilder;

        public GameRooms(
            IGameRoomsViewModelBuilder gameRoomsViewModelBuilder)
        {
            this.gameRoomsViewModelBuilder = gameRoomsViewModelBuilder;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(Guid gameId, PageType pageType, Guid? pageId)
        {
            var moduleRoomsViewModel = await gameRoomsViewModelBuilder.Build(gameId, pageType, pageId);
            return View("~/Views/GameActions/GameRooms/GameRooms.cshtml", moduleRoomsViewModel);
        }
    }
}