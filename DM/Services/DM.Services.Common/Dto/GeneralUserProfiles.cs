using System.Linq;
using AutoMapper;
using DM.Services.Authentication.Dto;
using DM.Services.Core.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;
using DbUserSettings = DM.Services.DataAccess.BusinessObjects.Users.Settings.UserSettings;
using DbPagingSettings = DM.Services.DataAccess.BusinessObjects.Users.Settings.PagingSettings;

namespace DM.Services.Common.Dto;

/// <summary>
/// Profile for user mapping
/// </summary>
internal class GeneralUserProfiles : Profile
{
    /// <inheritdoc />
    public GeneralUserProfiles()
    {
        CreateMap<User, GeneralUser>()
            .ForMember(d => d.OriginalPictureUrl, s => s.MapFrom(u => u.ProfilePictureUrl))
            .ForMember(d => d.MediumPictureUrl, s => s.MapFrom(u => u.MediumProfilePictureUrl))
            .ForMember(d => d.SmallPictureUrl, s => s.MapFrom(u => u.SmallProfilePictureUrl));
        CreateMap<User, AuthenticatedUser>()
            .ForMember(d => d.AccessRestrictionPolicies, s => s.MapFrom(
                u => u.BansReceived
                    .Where(b => !b.IsRemoved)
                    .Select(b => b.AccessRestrictionPolicy)
                    .ToList()));

        CreateMap<DbUserSettings, UserSettings>()
            .ForMember(d => d.Id, s => s.MapFrom(u => u.UserId));
        CreateMap<DbPagingSettings, PagingSettings>();
    }
}