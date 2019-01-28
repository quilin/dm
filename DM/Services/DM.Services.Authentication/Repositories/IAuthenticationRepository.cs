using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Authentication.Repositories
{
    public interface IAuthenticationRepository
    {
        Task<(bool Success, AuthenticatingUser User)> TryFindUser(string login);
        Task<AuthenticatingUser> FindUser(Guid userId);
        Task<Session> FindUserSession(Guid sessionId);
    }
}