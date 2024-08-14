using DM.Services.Authentication.Dto;
using Serilog.Context;

namespace DM.Services.Authentication.Implementation.UserIdentity;

/// <summary>
/// Current user identity storage
/// </summary>
internal class IdentityProvider : IIdentitySetter, IIdentityProvider
{
    private IIdentity identity;

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