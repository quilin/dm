using System.Linq;
using AutoMapper;
using DM.Services.Gaming.Dto.Internal;
using DM.Services.Gaming.Dto.Output;
using DbCharacter = DM.Services.DataAccess.BusinessObjects.Games.Characters.Character;
using DbAttribute = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.CharacterAttribute;

namespace DM.Services.Gaming.Dto
{
    /// <summary>
    /// Profile for character DTO and DAL mapping
    /// </summary>
    public class CharacterProfile : Profile
    {
        /// <inheritdoc />
        public CharacterProfile()
        {
            CreateMap<DbCharacter, Character>()
                .ForMember(d => d.Id, s => s.MapFrom(c => c.CharacterId))
                .ForMember(d => d.PictureUrl, s => s.MapFrom(c => c.Pictures
                    .Where(p => !p.IsRemoved)
                    .Select(p => p.VirtualPath)
                    .FirstOrDefault()));

            CreateMap<DbAttribute, CharacterAttribute>()
                .ForMember(d => d.Id, s => s.MapFrom(a => a.AttributeId))
                .ForMember(d => d.Value, s => s.MapFrom(a => a.Value));

            CreateMap<DbCharacter, CharacterToUpdate>()
                .ForMember(d => d.GameMasterId, s => s.MapFrom(c => c.Game.MasterId))
                .ForMember(d => d.GameAssistantId, s => s.MapFrom(c => c.Game.AssistantId));
        }
    }
}