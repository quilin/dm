using AutoMapper;
using DM.Services.Community.Dto;
using DM.Services.Core.Dto;
using DM.Web.Core.Extensions.EnumExtensions;

namespace DM.Web.API.Dto.Users
{
    /// <summary>
    /// Mapping profile from Service DTO to API DTO for users
    /// </summary>
    public class UserProfile : Profile
    {
        /// <inheritdoc />
        public UserProfile()
        {
            CreateMap<GeneralUser, User>()
                .ForMember(d => d.Roles, s => s.MapFrom(u => u.Role.GetUserRoles()))
                .ForMember(d => d.Online, s => s.MapFrom(u => u.LastVisitDate))
                .ForMember(d => d.Rating, s => s.MapFrom(u => new Rating
                {
                    Enabled = !u.RatingDisabled,
                    Quality = u.QualityRating,
                    Quantity = u.QuantityRating
                }));

            CreateMap<Registration, UserRegistration>();
            CreateMap<ResetPassword, UserPasswordReset>();
        }
    }
}