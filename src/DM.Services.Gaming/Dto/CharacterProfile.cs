using System.Linq;
using AutoMapper;
using DM.Services.Gaming.Dto.Internal;
using DM.Services.Gaming.Dto.Output;
using DbCharacter = DM.Services.DataAccess.BusinessObjects.Games.Characters.Character;
using DbAttribute = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.CharacterAttribute;
using DbPost = DM.Services.DataAccess.BusinessObjects.Games.Posts.Post;

namespace DM.Services.Gaming.Dto;

/// <summary>
/// Profile for character DTO and DAL mapping
/// </summary>
internal class CharacterProfile : Profile
{
    /// <inheritdoc />
    public CharacterProfile()
    {
        CreateMap<DbCharacter, Character>()
            .ForMember(d => d.Id, s => s.MapFrom(c => c.CharacterId))
            .ForMember(d => d.PictureUrl, s => s.MapFrom(c => c.Pictures
                .Where(p => !p.IsRemoved)
                .Select(p => p.FilePath)
                .FirstOrDefault()))
            .ForMember(d => d.TotalPostsCount, s => s.MapFrom(c => c.Posts
                .Count(p => !p.IsRemoved)));

        CreateMap<DbAttribute, CharacterAttribute>()
            .ForMember(d => d.Id, s => s.MapFrom(a => a.AttributeId));

        CreateMap<DbCharacter, CharacterToUpdate>()
            .ForMember(d => d.GameMasterId, s => s.MapFrom(c => c.Game.MasterId))
            .ForMember(d => d.GameAssistantId, s => s.MapFrom(c => c.Game.AssistantId));

        CreateMap<DbCharacter, CharacterShort>()
            .Include<DbCharacter, CharacterShortInfo>()
            .ForMember(d => d.Id, s => s.MapFrom(c => c.CharacterId))
            .ForMember(d => d.PictureUrl, s => s.MapFrom(c => c.Pictures
                .Where(p => !p.IsRemoved)
                .Select(p => p.FilePath)
                .FirstOrDefault()));

        CreateMap<DbPost, LastPost>()
            .ForMember(d => d.Id, s => s.MapFrom(p => p.PostId))
            .ForMember(d => d.CreateDate, s => s.MapFrom(p => p.CreateDate))
            .ForMember(d => d.RoomId, s => s.MapFrom(p => p.RoomId));
        CreateMap<DbCharacter, CharacterShortInfo>()
            .ForMember(d => d.LastPost, s => s.MapFrom(c => c.Posts
                .Where(p => !p.IsRemoved)
                .OrderByDescending(p => p.CreateDate)
                .FirstOrDefault()))
            .ForMember(d => d.PostsCount, s => s.MapFrom(c => c.Posts.Count(p => !p.IsRemoved)));
    }
}