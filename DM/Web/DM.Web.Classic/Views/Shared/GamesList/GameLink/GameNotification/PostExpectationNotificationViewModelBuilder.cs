using System.Linq;
using DM.Services.Gaming.Dto.Output;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Shared.GamesList.GameLink.GameNotification
{
    public class PostExpectationNotificationViewModelBuilder : IPostExpectationNotificationViewModelBuilder
    {
        private readonly IUserViewModelBuilder userViewModelBuilder;

        public PostExpectationNotificationViewModelBuilder(
            IUserViewModelBuilder userViewModelBuilder)
        {
            this.userViewModelBuilder = userViewModelBuilder;
        }

        public PostExpectationNotificationViewModel Build(Game game) => new PostExpectationNotificationViewModel
        {
            RoomId = game.Pendings.OrderBy(p => p.WaitsSince).First().RoomId,
            Authors = game.Pendings.Select(p => userViewModelBuilder.Build(p.AwaitingUser))
        };

        public PostExpectationNotificationViewModel Build(Room room)
        {
            return new PostExpectationNotificationViewModel
            {
                RoomId = room.Id,
                Authors = room.Pendings.Select(p => userViewModelBuilder.Build(p.AwaitingUser))
            };
        }
    }
}