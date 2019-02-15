using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Fora
{
    public interface IModeratorsApiService
    {
        Task<ListEnvelope<User>> GetModerators(string id);
    }
}