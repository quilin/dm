using System.Linq;
using AutoMapper;
using DM.Services.Gaming.Dto.Output;
using DbGame = DM.Services.DataAccess.BusinessObjects.Games.Game;
using DbGameTag = DM.Services.DataAccess.BusinessObjects.Common.Tag;

namespace DM.Services.Gaming.Dto
{
    /// <summary>
    /// Profile for game DTO and DAL mapping
    /// </summary>
    public class GameProfile : Profile
    {
        /// <inheritdoc />
        public GameProfile()
        {
            CreateMap<DbGame, Game>()
                .ForMember(d => d.Id, s => s.MapFrom(g => g.GameId));
            CreateMap<DbGame, GameExtended>()
                .ForMember(d => d.Id, s => s.MapFrom(g => g.GameId))
                .ForMember(d => d.Tags, s => s.MapFrom(g => g.GameTags.Select(t => t.Tag)));
            CreateMap<DbGameTag, GameTag>()
                .ForMember(d => d.Id, s => s.MapFrom(g => g.TagId))
                .ForMember(d => d.GroupTitle, s => s.MapFrom(g => g.TagGroup.Title));
        }
    }
}