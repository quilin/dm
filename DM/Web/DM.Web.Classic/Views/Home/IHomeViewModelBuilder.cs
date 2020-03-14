using System.Threading.Tasks;

namespace DM.Web.Classic.Views.Home
{
    public interface IHomeViewModelBuilder
    {
        Task<HomeViewModel> Build();
    }
}