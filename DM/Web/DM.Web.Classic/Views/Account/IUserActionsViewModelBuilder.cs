using System.Threading.Tasks;

namespace DM.Web.Classic.Views.Account
{
    public interface IUserActionsViewModelBuilder
    {
        Task<UserActionsViewModel> Build(string login);
    }
}