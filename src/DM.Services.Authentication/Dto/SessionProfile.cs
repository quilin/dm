using AutoMapper;

namespace DM.Services.Authentication.Dto;

/// <inheritdoc />
internal class SessionProfile : Profile
{
    /// <inheritdoc />
    public SessionProfile()
    {
        CreateMap<DataAccess.BusinessObjects.Users.Session, Session>();
    }
}