using DM.Services.Authentication.Dto;

namespace DM.Services.Authentication.Implementation
{
    public interface IUserProvider
    {
        IntendingUser Current { get; }
    }
}