using System.Threading.Tasks;

namespace DM.Web.Classic.Views.Home
{
    public interface IAboutViewModelBuilder
    {
        Task<AboutViewModel> Build();
    }
}