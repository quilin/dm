using AutoMapper;
using DM.Services.Gaming.Dto.Internal;
using DM.Services.Gaming.Dto.Output;
using DbRoom = DM.Services.DataAccess.BusinessObjects.Games.Posts.Room;
using DbPendingPost = DM.Services.DataAccess.BusinessObjects.Games.Links.PendingPost;
using PendingPost = DM.Services.Gaming.Dto.Output.PendingPost;
using RoomClaim = DM.Services.DataAccess.BusinessObjects.Games.Links.RoomClaim;

namespace DM.Services.Gaming.Dto
{
    /// <inheritdoc />
    public class RoomProfile : Profile
    {
        /// <inheritdoc />
        public RoomProfile()
        {
            CreateMap<DbRoom, RoomToUpdate>();
            CreateMap<DbRoom, Room>()
                .Include<DbRoom, RoomToUpdate>()
                .ForMember(d => d.Id, s => s.MapFrom(r => r.RoomId))
                .ForMember(d => d.Claims, s => s.MapFrom(r => r.ParticipantLinks))
                .ForMember(d => d.Pendings, s => s.MapFrom(r => r.PendingPosts));

            CreateMap<DbRoom, RoomOrderInfo>()
                .ForMember(d => d.Id, s => s.MapFrom(r => r.RoomId));

            CreateMap<RoomClaim, Output.RoomClaim>()
                .ForMember(d => d.Id, s => s.MapFrom(l => l.RoomClaimId));
            CreateMap<DbPendingPost, PendingPost>()
                .ForMember(d => d.Id, s => s.MapFrom(p => p.PostPendingId))
                .ForMember(d => d.AwaitingUser, s => s.MapFrom(p => p.AwaitingUser))
                .ForMember(d => d.PendingUser, s => s.MapFrom(p => p.PendingUser))
                .ForMember(d => d.WaitsSince, s => s.MapFrom(p => p.CreateDate));
        }
    }
}