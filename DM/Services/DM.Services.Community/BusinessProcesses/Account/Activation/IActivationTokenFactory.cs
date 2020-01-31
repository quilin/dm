using System;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Community.BusinessProcesses.Account.Activation
{
    /// <summary>
    /// Factory for activation tokens
    /// </summary>
    public interface IActivationTokenFactory
    {
        /// <summary>
        /// Create new activation token for account
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Token DAL</returns>
        Token Create(Guid userId);
    }
}