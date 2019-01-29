using DM.Services.Authentication.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.BusinessObjects.Users.Settings;

namespace DM.Services.Authentication.Implementation
{
    public interface IUserSetter
    {
        AuthenticatedUser Current { set; }
        Session CurrentSession { set; }
        UserSettings CurrentSettings { set; }
    }
}