using DtoRoom = DM.Services.Gaming.Dto.Output.Room;

namespace DM.Web.Classic.Views.EditRoom
{
    public interface IEditRoomFormBuilder
    {
        EditRoomForm Build(DtoRoom room);
    }
}