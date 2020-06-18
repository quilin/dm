using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DM.Services.Community.BusinessProcesses.Account.EmailChange;
using DM.Services.Community.BusinessProcesses.Account.PasswordChange;
using DM.Services.Community.BusinessProcesses.Account.PasswordReset;
using DM.Services.Community.BusinessProcesses.Account.Registration;
using DM.Services.Community.BusinessProcesses.Users.Reading;
using DM.Services.Community.BusinessProcesses.Users.Updating;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;

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
            CreateMap<UserRole, IEnumerable<string>>()
                .ConvertUsing(userRole =>
                    Enum.GetValues(typeof(UserRole))
                        .Cast<UserRole>()
                        .Where(role => role != UserRole.Guest && role != UserRole.Player)
                        .Where(role => userRole.HasFlag(role))
                        .Select(role => role.ToString()));

            CreateMap<GeneralUser, User>()
                .ForMember(d => d.Roles, s => s.MapFrom(u => u.Role))
                .ForMember(d => d.Online, s => s.MapFrom(u => u.LastVisitDate))
                .ForMember(d => d.Rating, s => s.MapFrom(u => new Rating
                {
                    Enabled = !u.RatingDisabled,
                    Quality = u.QualityRating,
                    Quantity = u.QuantityRating
                }));

            CreateMap<UserDetails, User>().IncludeBase<GeneralUser, User>();
            CreateMap<DM.Services.Authentication.Dto.UserSettings, UserSettings>().ReverseMap();
            CreateMap<DM.Services.Authentication.Dto.PagingSettings, PagingSettings>().ReverseMap();

            CreateMap<Registration, UserRegistration>();
            CreateMap<ResetPassword, UserPasswordReset>();
            CreateMap<ChangePassword, UserPasswordChange>();
            CreateMap<ChangeEmail, UserEmailChange>();

            CreateMap<User, UpdateUser>();
        }
    }
}