using DM.Services.Authentication.Dto;

namespace DM.Services.Authentication.Implementation
{
    /// <summary>
    /// Provides the current user identity
    /// </summary>
    public interface IIdentityProvider
    {
        /// <summary>
        /// Current user identity
        /// </summary>
        IIdentity Current { get; }
    }
}