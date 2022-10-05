using AutoMapper;
using DM.Services.Gaming.Dto.Input;

namespace DM.Web.API.Dto.Games;

/// <summary>
/// Mapping profile for game models
/// </summary>
internal class RoomProfile : Profile
{
    /// <inheritdoc />
    public RoomProfile()
    {
        CreateMap<DM.Services.Gaming.Dto.Output.Room, Room>();

        CreateMap<Room, CreateRoom>();
        CreateMap<Room, UpdateRoom>();

        CreateMap<DM.Services.Gaming.Dto.Output.RoomClaim, RoomClaim>();

        CreateMap<RoomClaim, CreateRoomClaim>()
            .ForMember(d => d.CharacterId, s => s.MapFrom(r => r.Character.Id))
            .ForMember(d => d.ReaderLogin, s => s.MapFrom(r => r.User != null ? r.User.Login : null));
        CreateMap<RoomClaim, UpdateRoomClaim>()
            .ForMember(d => d.ClaimId, s => s.MapFrom(r => r.Id));

        CreateMap<DM.Services.Gaming.Dto.Output.PendingPost, PendingPost>()
            .ForMember(d => d.Awaiting, s => s.MapFrom(p => p.AwaitingUser))
            .ForMember(d => d.Pending, s => s.MapFrom(p => p.PendingUser));

        CreateMap<PendingPost, CreatePendingPost>()
            .ForMember(d => d.PendingUserLogin, s => s.MapFrom(p => p.Pending.Login));
    }
}