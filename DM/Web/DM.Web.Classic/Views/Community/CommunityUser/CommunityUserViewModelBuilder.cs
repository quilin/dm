using DM.Services.Core.Dto;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Community.CommunityUser
{
    public class CommunityUserViewModelBuilder : ICommunityUserViewModelBuilder
    {
        private readonly IUserViewModelBuilder userViewModelBuilder;

        public CommunityUserViewModelBuilder(
            IUserViewModelBuilder userViewModelBuilder)
        {
            this.userViewModelBuilder = userViewModelBuilder;
        }

        public CommunityUserViewModel Build(GeneralUser user, int index)
        {
            return new CommunityUserViewModel
            {
                UserId = user.UserId,
                User = userViewModelBuilder.Build(user),
                Number = index
            };
        }
    }
}