using AutoMapper;
using DM.Services.Gaming.Dto.Input;
using DM.Web.Core.Helpers;

namespace DM.Web.API.Dto.Games
{
    /// <summary>
    /// Mapping profile for game models
    /// </summary>
    public class RoomProfile : Profile
    {
        /// <inheritdoc />
        public RoomProfile()
        {
            CreateMap<DM.Services.Gaming.Dto.Output.Room, Room>()
                .ForMember(d => d.Id, s => s.MapFrom(r => r.Id.EncodeToReadable(r.Title)))
                .ForMember(d => d.PreviousRoomId, s => s.MapFrom(r =>
                    r.PreviousRoomId.HasValue
                        ? r.PreviousRoomId.Value.EncodeToReadable(string.Empty)
                        : string.Empty));

            CreateMap<Room, CreateRoom>();
            CreateMap<Room, UpdateRoom>();
        }
    }
}