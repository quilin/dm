using AutoMapper;
using DM.Services.Core.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Community.BusinessProcesses.Users.Reading;

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