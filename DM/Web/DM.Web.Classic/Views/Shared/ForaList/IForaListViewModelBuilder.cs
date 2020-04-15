using System;
using System.Threading.Tasks;

namespace DM.Web.Classic.Views.Shared.ForaList
{
    public interface IForaListViewModelBuilder
    {
        Task<ForaListViewModel> Build(Guid? forumId);
    }
}