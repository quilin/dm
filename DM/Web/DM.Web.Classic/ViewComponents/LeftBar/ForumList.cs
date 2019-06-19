using System;
using System.Threading.Tasks;
using DM.Web.Classic.Views.Shared.ForaList;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.ViewComponents.LeftBar
{
    public class ForumList : ViewComponent
    {
        private readonly IForaListViewModelBuilder foraListViewModelBuilder;

        public ForumList(
            IForaListViewModelBuilder foraListViewModelBuilder)
        {
            this.foraListViewModelBuilder = foraListViewModelBuilder;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid? forumId)
        {
            return await Task.Run(() =>
            {
                var forumsListViewModel = foraListViewModelBuilder.Build(forumId);
                return View("~/Views/Shared/ForaList/ForaList.cshtml", forumsListViewModel);
            });
        }
    }
}