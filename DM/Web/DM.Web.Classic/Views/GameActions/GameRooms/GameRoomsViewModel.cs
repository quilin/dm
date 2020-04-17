using System.Collections.Generic;

namespace DM.Web.Classic.Views.GameActions.GameRooms
{
    public class GameRoomsViewModel
    {
        public bool IsDefaultRoom { get; set; }
        public IEnumerable<RoomLinkViewModel> Rooms { get; set; }
        public PageType PageType { get; set; }
    }
}