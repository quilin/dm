using DM.Services.Authentication.Dto;

namespace DM.Services.Authentication.Implementation.UserIdentity;

/// <summary>
/// Stores current user identity
/// </summary>
public interface IIdentitySetter
{
    /// <summary>
    /// Current user identity
    /// </summary>
    IIdentity Current { set; }

    /// <summary>
    /// Refresh current identity
    /// That is the workaround for Serilog enricher lost problem for middleware
    /// </summary>
    void Refresh();
}