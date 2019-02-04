using DM.Services.Authentication.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Authentication.Implementation
{
    public interface IUserSetter
    {
        AuthenticatedUser Current { set; }
        Session CurrentSession { set; }
        UserSettings CurrentSettings { set; }
    }
}