using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;

namespace DM.Web.Classic.Views.Shared.User
{
    public class UserViewModelBuilder : IUserViewModelBuilder
    {
        private readonly IUserRatingFactory userRatingFactory;

        public UserViewModelBuilder(
            IUserRatingFactory userRatingFactory
        )
        {
            this.userRatingFactory = userRatingFactory;
        }

        public UserViewModel Build(GeneralUser user)
        {
            return new UserViewModel
            {
                Login = user.Login,
                Name = user.Name,
                Location = user.Location,
                Status = user.Status,
                IsOnline = user.LastVisitDate.HasValue,
                Rating = userRatingFactory.Create(user),
                ProfilePictureUrl = user.PictureUrl,

                IsAdministrator = user.Role.HasFlag(UserRole.Administrator),
                IsModerator = user.Role.HasFlag(UserRole.RegularModerator) || user.Role.HasFlag(UserRole.SeniorModerator)
            };
        }
    }
}