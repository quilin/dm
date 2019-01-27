using DM.Services.Authentication.Dto;

namespace DM.Services.Authentication.Implementation
{
    public class UserProvider : IUserProvider, IUserSetter
    {
        public IntendingUser Current { get; set; }
    }
}