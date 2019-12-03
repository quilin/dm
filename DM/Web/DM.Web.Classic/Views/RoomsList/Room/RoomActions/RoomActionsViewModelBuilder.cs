using DtoRoom = DM.Services.Gaming.Dto.Output.Room;

namespace DM.Web.Classic.Views.RoomsList.Room.RoomActions
{
    public class RoomActionsViewModelBuilder : IRoomActionsViewModelBuilder
    {
        public RoomActionsViewModel Build(DtoRoom room)
        {
            return new RoomActionsViewModel
            {
                RoomId = room.Id,
                RoomTitle = room.Title
            };
        }
    }
}