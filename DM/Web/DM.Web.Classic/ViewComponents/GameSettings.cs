using System;
using System.Threading.Tasks;
using DM.Web.Classic.Views.EditGame;
using DM.Web.Classic.Views.GameNotepad;
using DM.Web.Classic.Views.GameRemove;
using DM.Web.Classic.Views.GameSettings;
using DM.Web.Classic.Views.RoomsList;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.ViewComponents
{
    public class GameSettings : ViewComponent
    {
        private readonly IEditGameViewModelBuilder editGameViewModelBuilder;
        private readonly IRoomsListViewModelBuilder roomsListViewModelBuilder;
        private readonly IGameNotepadFormBuilder gameNotepadFormBuilder;
        private readonly IGameRemoveViewModelBuilder gameRemoveViewModelBuilder;

        public GameSettings(
            IEditGameViewModelBuilder editGameViewModelBuilder,
            IRoomsListViewModelBuilder roomsListViewModelBuilder,
            IGameNotepadFormBuilder gameNotepadFormBuilder,
            IGameRemoveViewModelBuilder gameRemoveViewModelBuilder)
        {
            this.editGameViewModelBuilder = editGameViewModelBuilder;
            this.roomsListViewModelBuilder = roomsListViewModelBuilder;
            this.gameNotepadFormBuilder = gameNotepadFormBuilder;
            this.gameRemoveViewModelBuilder = gameRemoveViewModelBuilder;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(Guid gameId, GameSettingsType settingsType)
        {
            switch (settingsType)
            {
                case GameSettingsType.General:
                    var editModuleViewModel = await editGameViewModelBuilder.Build(gameId);
                    return View("~/Views/EditGame/EditGame.cshtml", editModuleViewModel);
                case GameSettingsType.Rooms:
                    var roomsListViewModel = await roomsListViewModelBuilder.Build(gameId);
                    return View("~/Views/RoomsList/RoomsList.cshtml", roomsListViewModel);
                case GameSettingsType.Attributes:
                    throw new NotImplementedException();
                case GameSettingsType.Notepad:
                    var moduleNotepadForm = await gameNotepadFormBuilder.Build(gameId);
                    return View("~/Views/GameNotepad/GameNotepad.cshtml", moduleNotepadForm);
                case GameSettingsType.BlackList:
                    throw new NotImplementedException();
                case GameSettingsType.Remove:
                    var moduleRemoveViewModel = await gameRemoveViewModelBuilder.Build(gameId);
                    return View("~/Views/GameRemove/RemoveForm.cshtml", moduleRemoveViewModel);
                default:
                    throw new ArgumentOutOfRangeException(nameof(settingsType), settingsType, null);
            }
        }
    }
}