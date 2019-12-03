using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Output;

namespace DM.Web.Classic.Views.EditGame
{
    public interface IEditGameViewModelBuilder
    {
        Task<EditGameViewModel> Build(GameExtended game);
    }
}