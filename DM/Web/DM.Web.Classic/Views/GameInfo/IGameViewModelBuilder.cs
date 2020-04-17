using System;
using System.Threading.Tasks;

namespace DM.Web.Classic.Views.GameInfo
{
    public interface IGameViewModelBuilder
    {
        Task<GameViewModel> Build(Guid gameId);
    }
}