using AutoMapper;
using DM.Services.Community.BusinessProcesses.Users.Reading;
using DM.Services.Core.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Community.Storage.Profiles;

/// <inheritdoc />
internal class ReadingProfile : Profile
{
    /// <inheritdoc />
    public ReadingProfile()
    {
        CreateMap<User, UserDetails>()
            .IncludeBase<User, GeneralUser>();
    }
}