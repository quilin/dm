using System;

namespace DM.Web.Classic.Views.RoomsList.CreateRoom
{
    public interface ICreateRoomFormBuilder
    {
        CreateRoomForm Build(Guid moduleId);
    }
}