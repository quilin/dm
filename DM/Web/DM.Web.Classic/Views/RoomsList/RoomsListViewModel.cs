using System;
using DM.Web.Classic.Views.RoomsList.Character;
using DM.Web.Classic.Views.RoomsList.CreateRoom;
using DM.Web.Classic.Views.RoomsList.Room;

namespace DM.Web.Classic.Views.RoomsList
{
    public class RoomsListViewModel
    {
        public Guid GameId { get; set; }
        public RoomViewModel[] Rooms { get; set; }
        public CharacterViewModel[] Characters { get; set; }
        public CreateRoomForm CreateRoomForm { get; set; }
    }
}