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

            CreateMap<DM.Services.Gaming.Dto.Output.RoomClaim, RoomClaim>()
                .ForMember(d => d.Id, s => s.MapFrom(r => r.Id.EncodeToReadable(string.Empty)));

            CreateMap<RoomClaim, CreateRoomClaim>()
                .ForMember(d => d.CharacterId, s => s.MapFrom(r => r.Character != null ? r.Character.Id : null))
                .ForMember(d => d.ReaderLogin, s => s.MapFrom(r => r.User != null ? r.User.Login : null));
            CreateMap<RoomClaim, UpdateRoomClaim>()
                .ForMember(d => d.ClaimId, s => s.MapFrom(r => r.Id));
        }
    }
}