using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;
using DbSession = DM.Services.DataAccess.BusinessObjects.Users.Session;
using Session = DM.Services.Authentication.Dto.Session;

namespace DM.Services.Authentication.Repositories;

/// <summary>
/// Authentication information storage
/// </summary>
internal interface IAuthenticationRepository
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
    Task RefreshSession(Guid userId, Guid sessionId, DateTimeOffset expirationDate);

    /// <summary>
    /// Append new session to user authentication sessions
    /// </summary>
    /// <param name="userId">Authenticated user id</param>
    /// <param name="session">Authentication session</param>
    /// <returns></returns>
    Task<Session> AddSession(Guid userId, DbSession session);

    /// <summary>
    /// Remove all sessions from the user except one
    /// </summary>
    /// <param name="userId">Authenticated user id</param>
    /// <param name="sessionId">Session id</param>
    /// <returns></returns>
    Task RemoveSessionsExcept(Guid userId, Guid sessionId);

    /// <summary>
    /// Update user last activity date
    /// </summary>
    /// <param name="userUpdate"></param>
    /// <returns></returns>
    Task UpdateActivity(IUpdateBuilder<User> userUpdate);
}