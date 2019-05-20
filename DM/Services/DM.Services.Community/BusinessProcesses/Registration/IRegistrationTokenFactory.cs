using System;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Community.BusinessProcesses.Registration
{
    /// <summary>
    /// Factory for registration tokens
    /// </summary>
    public interface IRegistrationTokenFactory
    {
        /// <summary>
        /// Create new registration token for account activation
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Token DAL</returns>
        Token Create(Guid userId);
    }
}