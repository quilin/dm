using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Forum.BusinessProcesses.Fora;

namespace DM.Web.Classic.Views.Shared.ForaList
{
    public class ForaListViewModelBuilder : IForaListViewModelBuilder
    {
        private readonly IForumReadingService forumService;
        private readonly IForaListItemViewModelBuilder foraListItemViewModelBuilder;

        public ForaListViewModelBuilder(
            IForumReadingService forumService,
            IForaListItemViewModelBuilder foraListItemViewModelBuilder)
        {
            this.forumService = forumService;
            this.foraListItemViewModelBuilder = foraListItemViewModelBuilder;
            this.forumService = forumService;
        }

        public async Task<ForaListViewModel> Build(Guid? forumId)
        {
            var forums = await forumService.GetForaList();
            return new ForaListViewModel
            {
                Fora = forums.Select(f => foraListItemViewModelBuilder.Build(f, forumId)).ToArray()
            };
        }
    }
}