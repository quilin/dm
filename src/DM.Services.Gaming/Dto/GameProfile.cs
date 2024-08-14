using System.Linq;
using AutoMapper;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.Gaming.Dto.Output;
using DbGame = DM.Services.DataAccess.BusinessObjects.Games.Game;
using DbGameTag = DM.Services.DataAccess.BusinessObjects.Common.Tag;
using GameTag = DM.Services.Gaming.Dto.Output.GameTag;

namespace DM.Services.Gaming.Dto;

/// <summary>
/// Profile for game DTO and DAL mapping
/// </summary>
internal class GameProfile : Profile
{
    /// <inheritdoc />
    public GameProfile()
    {
        CreateMap<DbGame, Game>()
            .Include<DbGame, GameExtended>()
            .ForMember(d => d.Id, s => s.MapFrom(g => g.GameId))
            .ForMember(d => d.Tags, s => s.MapFrom(g => g.GameTags.Select(t => t.Tag)))
            .ForMember(d => d.PendingAssistant, s => s.MapFrom(g => g.Tokens
                .Where(t => !t.IsRemoved && t.Type == TokenType.AssistantAssignment)
                .Select(t => t.User)
                .FirstOrDefault()))
            .ForMember(d => d.ActiveCharacterUserIds, s => s.MapFrom(g => g.Characters
                .Where(c => !c.IsRemoved && c.Status == CharacterStatus.Active)
                .Select(c => c.CharacterId)))
            .ForMember(d => d.ReaderUserIds, s => s.MapFrom(g => g.Readers
                .Select(r => r.UserId)))
            .ForMember(d => d.BlacklistedUsers, s => s.MapFrom(g => g.BlackList));

        CreateMap<BlackListLink, BlacklistedUser>()
            .ForMember(u => u.LinkId, s => s.MapFrom(l => l.BlackListLinkId));

        CreateMap<DbGame, GameExtended>()
            .ForMember(d => d.Readers, s => s.MapFrom(g => g.Readers.Select(r => r.User)))
            .ForMember(d => d.Characters, s => s.MapFrom(g => g.Characters.Where(c => !c.IsRemoved)));

        CreateMap<DbGameTag, GameTag>()
            .ForMember(d => d.Id, s => s.MapFrom(g => g.TagId))
            .ForMember(d => d.GroupTitle, s => s.MapFrom(g => g.TagGroup.Title));
    }
}