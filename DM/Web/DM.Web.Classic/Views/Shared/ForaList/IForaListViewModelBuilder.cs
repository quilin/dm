using System;

namespace DM.Web.Classic.Views.Shared.ForaList
{
    public interface IForaListViewModelBuilder
    {
        ForaListViewModel Build(Guid? forumId);
    }
}