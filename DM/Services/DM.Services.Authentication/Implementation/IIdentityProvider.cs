using DM.Services.Authentication.Dto;

namespace DM.Services.Authentication.Implementation
{
    public interface IIdentityProvider
    {
        AuthenticationResult Current { get; }
    }
}