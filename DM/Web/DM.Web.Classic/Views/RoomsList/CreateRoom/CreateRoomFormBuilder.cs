using System;

namespace DM.Web.Classic.Views.RoomsList.CreateRoom
{
    public class CreateRoomFormBuilder : ICreateRoomFormBuilder
    {
        public CreateRoomForm Build(Guid moduleId)
        {
            return new CreateRoomForm
                       {
                           GameId = moduleId
                       };
        }
    }
}