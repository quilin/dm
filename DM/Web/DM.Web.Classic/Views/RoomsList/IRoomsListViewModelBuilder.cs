using System;
using System.Threading.Tasks;

namespace DM.Web.Classic.Views.RoomsList
{
    public interface IRoomsListViewModelBuilder
    {
        Task<RoomsListViewModel> Build(Guid gameId);
    }
}