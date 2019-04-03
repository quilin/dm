using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Authentication.Repositories
{
    /// <summary>
    /// Authentication information storage
    /// </summary>
    public interface IAuthenticationRepository
    {
        /// <summary>
        /// Search for user by its login
        /// </summary>
        /// <param name="login">User login</param>
        /// <returns>Pair of operation success flag and the user data. If no user is found, the user will be null</returns>
        Task<(bool Success, AuthenticatedUser User)> TryFindUser(string login);

        /// <summary>
        /// Search for user by its id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>User data</returns>
        Task<AuthenticatedUser> FindUser(Guid userId);

        /// <summary>
        /// Search for authentication session by its id
        /// </summary>
        /// <param name="sessionId">Authentication session id</param>
        /// <returns>Session</returns>
        Task<Session> FindUserSession(Guid sessionId);

        /// <summary>
        /// Search for user settings by user id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>User settings</returns>
        Task<UserSettings> FindUserSettings(Guid userId);

        /// <summary>
        /// Remove authentication session
        /// </summary>
        /// <param name="userId">Authenticated user id</param>
        /// <param name="sessionId">Authentication session id</param>
        /// <returns></returns>
        Task RemoveSession(Guid userId, Guid sessionId);

        /// <summary>
        /// Update authentication session that is about to expire
        /// </summary>
        /// <param name="userId">Authenticated user id</param>
        /// <param name="sessionId">Authentication session id</param>
        /// <param name="expirationDate">New expiration date</param>
        /// <returns></returns>
        Task RefreshSession(Guid userId, Guid sessionId, DateTime expirationDate);

        /// <summary>
        /// Append new session to user authentication sessions
        /// </summary>
        /// <param name="userId">Authenticated user id</param>
        /// <param name="session">Authentication session</param>
        /// <returns></returns>
        Task AddSession(Guid userId, Session session);

        /// <summary>
        /// Remove all sessions from the user
        /// </summary>
        /// <param name="userId">Authenticated user id</param>
        /// <returns></returns>
        Task RemoveSessions(Guid userId);

        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="user">User DAL</param>
        /// <returns></returns>
        Task CreateUserWithRegistrationToken(User user);
    }
}