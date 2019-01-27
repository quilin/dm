using System.Threading.Tasks;
using DM.Services.UserServices.Dto;

namespace DM.Services.UserServices.Repositories
{
    public interface IAuthenticationRepository
    {
        Task<AuthenticatingUser> Find(string login);
    }
}