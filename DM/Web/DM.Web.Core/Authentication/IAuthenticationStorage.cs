using System.Threading.Tasks;
using DM.Services.Authentication.Dto;

namespace DM.Web.Core.Authentication
{
    public interface IAuthenticationStorage
    {
        Task Store(Identity identity);
    }
}