using DM.Services.Authentication.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Authentication.Implementation
{
    public interface IUserSetter
    {
        IntendingUser Current { set; }
        Session CurrentSession { set; }
    }
}