using DM.Services.Authentication.Dto;

namespace DM.Services.Authentication.Implementation
{
    public interface IUserSetter
    {
        IntendingUser Current { set; }
    }
}