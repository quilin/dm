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
                .ForMember(d => d.Id, s => s.MapFrom(g => g.GameId))
                .ForMember(d => d.UserParticipates, s => s.MapFrom<GameParticipationResolver>());

            CreateMap<DbGame, GameExtended>()
                .ForMember(d => d.Id, s => s.MapFrom(g => g.GameId))
                .ForMember(d => d.UserParticipates, s => s.MapFrom<GameParticipationResolver>())
                .ForMember(d => d.Tags, s => s.MapFrom(g => g.GameTags.Select(t => t.Tag)))
                .ForMember(d => d.PendingAssistant, s => s.MapFrom(g => g.Tokens
                    .Where(t => !t.IsRemoved)
                    .Select(t => t.User)
                    .FirstOrDefault()));

            CreateMap<DbGameTag, GameTag>()
                .ForMember(d => d.Id, s => s.MapFrom(g => g.TagId))
                .ForMember(d => d.GroupTitle, s => s.MapFrom(g => g.TagGroup.Title));
        }
    }
}