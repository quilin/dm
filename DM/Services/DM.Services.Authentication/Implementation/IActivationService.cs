using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;

namespace DM.Services.Authentication.Implementation
{
    /// <summary>
    /// Service for user activation
    /// </summary>
    public interface IActivationService
    {
        /// <summary>
        /// Activate user by token identifier
        /// </summary>
        /// <param name="tokenId">Activation token identifier</param>
        /// <returns></returns>
        Task<IIdentity> Activate(Guid tokenId);
    }
}