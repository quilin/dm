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
                // Name = user.Name,
                // Location = user.Location,
                // CanWriteMessage = intentionsManager.IsAllowed(UserIntention.WriteMessage, user as IUser),
                // CanLogIn = intentionsManager.IsAllowed(CommonIntention.LogAsAnyUser),
                Number = index
            };
        }
    }
}