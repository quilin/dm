using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Community.Dto;
using DM.Services.Core.Dto;

namespace DM.Services.Community.Repositories
{
    public interface IUserRepository
    {
        Task<int> CountUsers(bool withInactive);
        Task<IEnumerable<GeneralUser>> GetUsers(PagingData paging, bool withInactive);
        Task<GeneralUser> GetUser(string login);
        Task<UserProfile> GetProfile(string login);
    }
}