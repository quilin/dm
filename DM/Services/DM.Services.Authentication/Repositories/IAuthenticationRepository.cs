using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;

namespace DM.Services.Authentication.Repositories
{
    public interface IAuthenticationRepository
    {
        Task<AuthenticatingUser> Find(string login);
        Task<AuthenticatingUser> Find(Guid userId);
    }
}