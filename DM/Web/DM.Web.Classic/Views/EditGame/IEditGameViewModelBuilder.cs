using System;
using System.Threading.Tasks;

namespace DM.Web.Classic.Views.EditGame
{
    public interface IEditGameViewModelBuilder
    {
        Task<EditGameViewModel> Build(Guid gameId);
    }
}