using DM.Services.Authentication.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Authentication.Implementation
{
    public class UserProvider : IUserSetter, IUserProvider
    {
        public IntendingUser Current { get; set; }
        public Session CurrentSession { get; set; }
    }
}