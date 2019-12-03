using System;
using System.Threading.Tasks;

namespace DM.Web.Classic.Views.GameRemove
{
    public interface IGameRemoveViewModelBuilder
    {
        Task<GameRemoveViewModel> Build(Guid gameId);
    }
}