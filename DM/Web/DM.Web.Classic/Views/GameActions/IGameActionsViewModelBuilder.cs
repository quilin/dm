using System;
using System.Threading.Tasks;

namespace DM.Web.Classic.Views.GameActions
{
    public interface IGameActionsViewModelBuilder
    {
        Task<GameActionsViewModel> Build(Guid gameId, PageType pageType, Guid? pageId);
    }
}