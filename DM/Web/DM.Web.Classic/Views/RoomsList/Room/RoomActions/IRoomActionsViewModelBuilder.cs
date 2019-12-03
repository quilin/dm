using DtoRoom = DM.Services.Gaming.Dto.Output.Room;

namespace DM.Web.Classic.Views.RoomsList.Room.RoomActions
{
    public interface IRoomActionsViewModelBuilder
    {
        RoomActionsViewModel Build(DtoRoom room);
    }
}