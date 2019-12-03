using System.Threading.Tasks;

namespace DM.Web.Classic.Views.CreateGame
{
    public interface ICreateGameViewModelBuilder
    {
        Task<CreateGameViewModel> Build();
    }
}