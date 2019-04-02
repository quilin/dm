using DM.Services.Authentication.Dto;
using Serilog.Context;

namespace DM.Services.Authentication.Implementation.UserIdentity
{
    /// <summary>
    /// Current user identity storage
    /// </summary>
    public class IdentityProvider : IIdentitySetter, IIdentityProvider
    {
        private IIdentity identity;

        /// <inheritdoc cref="IdentityProvider" />
        public IIdentity Current
        {
            get => identity;
            set
            {
                identity = value;
                var userName = identity.User.IsAuthenticated ? identity.User.Login : "Guest";
                LogContext.PushProperty("User", userName);
            }
        }

        /// <inheritdoc />
        public void Refresh() => Current = Current;
    }
}