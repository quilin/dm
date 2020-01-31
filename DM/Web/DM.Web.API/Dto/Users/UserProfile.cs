using AutoMapper;
using DM.Services.Community.BusinessProcesses.Account.EmailChange;
using DM.Services.Community.BusinessProcesses.Account.PasswordChange;
using DM.Services.Community.BusinessProcesses.Account.PasswordReset;
using DM.Services.Community.BusinessProcesses.Account.Registration;
using DM.Services.Community.BusinessProcesses.Users.Reading;
using DM.Services.Community.BusinessProcesses.Users.Updating;
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
            CreateMap<UserDetails, User>();
            CreateMap<DM.Services.Authentication.Dto.UserSettings, UserSettings>().ReverseMap();
            CreateMap<DM.Services.Authentication.Dto.PagingSettings, PagingSettings>().ReverseMap();

            CreateMap<GeneralUser, User>()
                .Include<UserDetails, User>()
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
            CreateMap<ChangePassword, UserPasswordChange>();
            CreateMap<ChangeEmail, UserEmailChange>();

            CreateMap<User, UpdateUser>();
        }
    }
}