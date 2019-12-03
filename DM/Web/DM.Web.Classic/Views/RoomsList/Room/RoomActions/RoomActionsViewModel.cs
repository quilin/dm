using System;

namespace DM.Web.Classic.Views.RoomsList.Room.RoomActions
{
    public class RoomActionsViewModel
    {
        public Guid RoomId { get; set; }
        public string RoomTitle { get; set; }
        public bool CanRemove { get; set; }
    }
}