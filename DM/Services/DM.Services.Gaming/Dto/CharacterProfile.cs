using System.Linq;
using AutoMapper;
using DM.Services.Gaming.Dto.Internal;
using DM.Services.Gaming.Dto.Output;
using DbCharacter = DM.Services.DataAccess.BusinessObjects.Games.Characters.Character;

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

            CreateMap<DbCharacter, CharacterToUpdate>()
                .ForMember(d => d.GameMasterId, s => s.MapFrom(c => c.Game.MasterId))
                .ForMember(d => d.GameAssistantId, s => s.MapFrom(c => c.Game.AssistantId));
        }
    }
}