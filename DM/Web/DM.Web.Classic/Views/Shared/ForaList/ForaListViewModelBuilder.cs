using System;
using System.Linq;
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

        public ForaListViewModel Build(Guid? forumId)
        {
            var forums = forumService.GetForaList().Result;
            return new ForaListViewModel
            {
                Fora = forums.Select(f => foraListItemViewModelBuilder.Build(f, forumId)).ToArray()
            };
        }
    }
}