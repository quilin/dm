using AutoMapper;
using DM.Services.Gaming.Dto.Input;
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
                .ForMember(c => c.Privacy, s => s.MapFrom<AccessPolicyConverter>());

            CreateMap<DM.Services.Gaming.Dto.Output.CharacterAttribute, CharacterAttribute>();

            CreateMap<Character, CreateCharacter>()
                .ForMember(c => c.IsNpc, s => s.MapFrom(c => c.Privacy.IsNpc))
                .ForMember(c => c.AccessPolicy, s => s.MapFrom<AccessPolicyConverter>());

            CreateMap<Character, UpdateCharacter>()
                .ForMember(c => c.IsNpc, s => s.MapFrom(c => c.Privacy.IsNpc))
                .ForMember(c => c.AccessPolicy, s => s.MapFrom<AccessPolicyConverter>());
        }

        private class AccessPolicyConverter :
            IValueResolver<Character, CreateCharacter, Policy>,
            IValueResolver<Character, UpdateCharacter, Policy?>,
            IValueResolver<DM.Services.Gaming.Dto.Output.Character, Character, CharacterPrivacySettings>
        {
            private static Policy Resolve(CharacterPrivacySettings privacySettings)
            {
                var result = Policy.NoAccess;
                if (privacySettings.EditByMaster)
                {
                    result |= Policy.EditAllowed;
                }

                if (privacySettings.EditPostByMaster)
                {
                    result |= Policy.PostEditAllowed;
                }

                return result;
            }

            public Policy Resolve(Character source, CreateCharacter destination, Policy destMember,
                ResolutionContext context) =>
                Resolve(source.Privacy);

            public Policy? Resolve(Character source, UpdateCharacter destination, Policy? destMember,
                ResolutionContext context) =>
                source.Privacy == null
                    ? (Policy?) null
                    : Resolve(source.Privacy);

            public CharacterPrivacySettings Resolve(DM.Services.Gaming.Dto.Output.Character source,
                Character destination, CharacterPrivacySettings destMember,
                ResolutionContext context)
            {
                return new CharacterPrivacySettings
                {
                    IsNpc = source.IsNpc,
                    EditByMaster = (source.AccessPolicy & Policy.EditAllowed) != Policy.NoAccess,
                    EditPostByMaster = (source.AccessPolicy & Policy.PostEditAllowed) != Policy.NoAccess
                };
            }
        }
    }
}