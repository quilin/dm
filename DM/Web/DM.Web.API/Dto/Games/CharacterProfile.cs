using AutoMapper;
using DM.Web.Core.Helpers;
using Policy = DM.Services.Core.Dto.Enums.CharacterAccessPolicy;

namespace DM.Web.API.Dto.Games
{
    /// <inheritdoc />
    public class CharacterProfile : Profile
    {
        /// <inheritdoc />
        public CharacterProfile()
        {
            CreateMap<DM.Services.Gaming.Dto.Output.Character, Character>()
                .ForMember(c => c.Id, s => s.MapFrom(c => c.Id.EncodeToReadable(c.Name)))
                .ForMember(c => c.Privacy, s => s.MapFrom(c => new CharacterPrivacySettings
                {
                    IsNpc = c.IsNpc,
                    EditByMaster = (c.AccessPolicy & Policy.EditAllowed) != Policy.NoAccess,
                    EditPostByMaster = (c.AccessPolicy & Policy.PostEditAllowed) != Policy.NoAccess
                }));
        }
    }
}