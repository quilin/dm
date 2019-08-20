using DM.Services.Authentication.Dto;
using Serilog.Context;

namespace DM.Services.Authentication.Implementation.UserIdentity
{
    /// <summary>
    /// Current user identity storage
    /// </summary>
    public class IdentityProvider : IIdentitySetter, IIdentityProvider
    {
        private IIdentity identity = Identity.Guest();

        /// <inheritdoc cref="IdentityProvider" />
        public IIdentity Current
        {
            get => identity;
            set
            {
                identity = value;
                LogContext.PushProperty("User", identity.User.Login);
            }
        }

        /// <inheritdoc />
        public void Refresh() => Current = Current;
    }
}