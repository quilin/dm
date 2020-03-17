using System.Threading.Tasks;

namespace DM.Web.Classic.Views.Profile
{
    public interface IProfileViewModelBuilder
    {
        Task<ProfileViewModel> Build(string login);
    }
}