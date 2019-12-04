using System;
using System.Threading.Tasks;

namespace DM.Web.Classic.Views.EditGameStatus
{
    public interface IEditGameStatusViewModelBuilder
    {
        Task<EditGameStatusViewModel> Build(Guid gameId);
    }
}