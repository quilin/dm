using DM.Services.Authentication.Dto;

namespace DM.Services.Authentication.Implementation
{
    /// <summary>
    /// Stores current user identity
    /// </summary>
    public interface IIdentitySetter
    {
        /// <summary>
        /// Current user identity
        /// </summary>
        IIdentity Current { set; }
    }
}