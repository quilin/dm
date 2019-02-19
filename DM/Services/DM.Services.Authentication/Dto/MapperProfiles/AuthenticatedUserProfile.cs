using System.Linq;
using AutoMapper;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Authentication.Dto.MapperProfiles
{
    public class AuthenticatedUserProfile : Profile
    {
        public AuthenticatedUserProfile()
        {
            CreateMap<User, AuthenticatedUser>()
                .ForMember(d => d.AccessRestrictionPolicies, s => s.MapFrom(
                    u => u.BansReceived.Where(b => !b.IsRemoved).Select(b => b.AccessRestrictionPolicy).ToList()));
        }
    }
}