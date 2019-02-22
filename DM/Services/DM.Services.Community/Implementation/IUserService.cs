using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Community.Dto;
using DM.Services.Core.Dto;

namespace DM.Services.Community.Implementation
{
    public interface IUserService
    {
        Task<(IEnumerable<GeneralUser> users, PagingData paging)> Get(int entityNumber, bool withInactive);
        Task<GeneralUser> Get(string login);
        Task<UserProfile> GetProfile(string login);
    }
}