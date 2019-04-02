using DM.Services.Authentication.Dto;
using DM.Services.Core.Dto.Enums;
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
                var userName = value.User.Role == UserRole.Guest ? "Guest" : value.User.Login;
                LogContext.PushProperty("User", userName);
            }
        }
    }
}