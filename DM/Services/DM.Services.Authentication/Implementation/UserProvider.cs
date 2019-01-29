using DM.Services.Authentication.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.BusinessObjects.Users.Settings;

namespace DM.Services.Authentication.Implementation
{
    public class UserProvider : IUserSetter, IUserProvider
    {
        public AuthenticatedUser Current { get; set; }
        public Session CurrentSession { get; set; }
        public UserSettings CurrentSettings { get; set; }
    }
}