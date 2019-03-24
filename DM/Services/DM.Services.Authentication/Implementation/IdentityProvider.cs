using DM.Services.Authentication.Dto;

namespace DM.Services.Authentication.Implementation
{
    /// <summary>
    /// Current user identity storage
    /// </summary>
    public class IdentityProvider : IIdentitySetter, IIdentityProvider
    {
        /// <inheritdoc cref="IdentityProvider" />
        public IIdentity Current { get; set; }
    }
}