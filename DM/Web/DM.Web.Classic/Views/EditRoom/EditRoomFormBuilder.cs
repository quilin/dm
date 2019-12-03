using DtoRoom = DM.Services.Gaming.Dto.Output.Room;

namespace DM.Web.Classic.Views.EditRoom
{
    public class EditRoomFormBuilder : IEditRoomFormBuilder
    {
        public EditRoomForm Build(DtoRoom room)
        {
            return new EditRoomForm
            {
                RoomId = room.Id,
                RoomTitle = room.Title,
                RoomAccess = room.AccessType,
                RoomType = room.Type
            };
        }
    }
}