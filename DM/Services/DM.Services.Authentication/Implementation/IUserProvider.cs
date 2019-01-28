using DM.Services.Authentication.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Authentication.Implementation
{
    public interface IUserProvider
    {
        IntendingUser Current { get; }
        Session CurrentSession { get; }
    }
}