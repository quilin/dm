using System;
using System.Threading.Tasks;

namespace DM.Web.Classic.Views.GameActions.GameRooms
{
    public interface IGameRoomsViewModelBuilder
    {
        Task<GameRoomsViewModel> Build(Guid gameId, PageType pageType, Guid? pageId);
    }
}