using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Authentication.Factories;

/// <summary>
/// Factory for a user session
/// </summary>
internal interface ISessionFactory
{
    /// <summary>
    /// Creates a session to be stored in DB
    /// </summary>
    /// <param name="persistent">Persistence flag</param>
    /// <param name="invisible">Invisibility flag</param>
    /// <returns></returns>
    Session Create(bool persistent, bool invisible);
}