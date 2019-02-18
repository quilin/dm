using System.Linq;
using AutoMapper;
using DM.Services.Core.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Common.MapperProfiles
{
    public class GeneralUserProfile : Profile
    {
        public GeneralUserProfile()
        {
            CreateMap<User, GeneralUser>()
                .ForMember(
                    d => d.ProfilePictureUrl,
                    s => s.MapFrom(u => u.ProfilePictures
                        .Where(p => !p.IsRemoved)
                        .Select(p => p.VirtualPath)
                        .FirstOrDefault()));
        }
    }
}