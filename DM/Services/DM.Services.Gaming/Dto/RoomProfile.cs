using AutoMapper;
using DM.Services.Gaming.Dto.Internal;
using DM.Services.Gaming.Dto.Output;
using DbRoom = DM.Services.DataAccess.BusinessObjects.Games.Posts.Room;

namespace DM.Services.Gaming.Dto
{
    /// <inheritdoc />
    public class RoomProfile : Profile
    {
        /// <inheritdoc />
        public RoomProfile()
        {
            CreateMap<DbRoom, Room>()
                .Include<DbRoom, RoomToUpdate>()
                .ForMember(d => d.Id, s => s.MapFrom(r => r.RoomId));

            CreateMap<DbRoom, RoomToUpdate>();

            CreateMap<DbRoom, RoomOrderInfo>()
                .ForMember(d => d.Id, s => s.MapFrom(r => r.RoomId));
        }
    }
}