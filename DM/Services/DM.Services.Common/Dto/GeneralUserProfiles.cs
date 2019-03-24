using System.Linq;
using AutoMapper;
using DM.Services.Authentication.Dto;
using DM.Services.Core.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Common.Dto
{
    /// <summary>
    /// Profile for user mapping
    /// </summary>
    public class GeneralUserProfiles : Profile
    {
        public GeneralUserProfiles()
        {
            CreateMap<User, GeneralUser>()
                .ForMember(
                    d => d.ProfilePictureUrl,
                    s => s.MapFrom(u => u.ProfilePictures
                        .Where(p => !p.IsRemoved)
                        .Select(p => p.VirtualPath)
                        .FirstOrDefault()));
            CreateMap<User, AuthenticatedUser>()
                .ForMember(d => d.AccessRestrictionPolicies, s => s.MapFrom(
                    u => u.BansReceived.Where(b => !b.IsRemoved).Select(b => b.AccessRestrictionPolicy).ToList()));
        }
    }
}