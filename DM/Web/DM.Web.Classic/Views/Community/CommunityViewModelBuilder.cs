using System.Linq;
using DM.Services.Community.BusinessProcesses.Reading;
using DM.Services.Core.Dto;
using DM.Web.Classic.Views.Community.CommunityUser;

namespace DM.Web.Classic.Views.Community
{
    public class CommunityViewModelBuilder : ICommunityViewModelBuilder
    {
        private readonly IUserReadingService userReadingService;
        private readonly ICommunityUserViewModelBuilder communityUserViewModelBuilder;

        private const int PageSize = 30;

        public CommunityViewModelBuilder(
            IUserReadingService userReadingService,
            ICommunityUserViewModelBuilder communityUserViewModelBuilder
        )
        {
            this.userReadingService = userReadingService;
            this.communityUserViewModelBuilder = communityUserViewModelBuilder;
        }

        public CommunityViewModel Build(int entityNumber, bool withInactive)
        {
            var (users, paging) = userReadingService.Get(new PagingQuery
            {
                Number = entityNumber,
                Size = PageSize
            }, withInactive).Result;
            var firstUserNumber = (paging.CurrentPage - 1) * PageSize + 1;

            return new CommunityViewModel
            {
                CurrentPage = paging.CurrentPage,
                TotalPagesCount = paging.TotalPagesCount,
                PageSize = paging.PageSize,
                EntityNumber = paging.EntityNumber,
                Administrators = new CommunityUserViewModel[0],
                Moderators = new CommunityUserViewModel[0],
                Users = users
                    .Select((u, i) => communityUserViewModelBuilder.Build(u, firstUserNumber + i))
                    .ToArray()
            };
        }

        public CommunityUserViewModel[] BuildList(int entityNumber, bool withInactive)
        {
            var (users, paging) = userReadingService.Get(new PagingQuery
            {
                Number = entityNumber,
                Size = PageSize
            }, withInactive).Result;
            var firstUserNumber = (paging.CurrentPage - 1) * PageSize + 1;

            return users
                .Select((u, i) => communityUserViewModelBuilder.Build(u, firstUserNumber + i))
                .ToArray();
        }
    }
}