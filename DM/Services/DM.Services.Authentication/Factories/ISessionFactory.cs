using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Authentication.Factories
{
    /// <summary>
    /// Factory for a user session
    /// </summary>
    public interface ISessionFactory
    {
        /// <summary>
        /// Creates a session to be stored in DB
        /// </summary>
        /// <param name="persistent">Persistence flag</param>
        /// <returns></returns>
        Session Create(bool persistent);
    }
}