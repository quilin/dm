using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Authentication.Repositories
{
    public interface IAuthenticationRepository
    {
        Task<(bool Success, AuthenticatedUser User)> TryFindUser(string login);
        Task<AuthenticatedUser> FindUser(Guid userId);
        Task<Session> FindUserSession(Guid sessionId);
        Task<UserSettings> FindUserSettings(Guid userId);

        Task RemoveSession(Guid userId, Guid sessionId);
        Task RefreshSession(Guid userId, Guid sessionId, DateTime expirationDate);
        Task AddSession(Guid userId, Session session);
    }
}