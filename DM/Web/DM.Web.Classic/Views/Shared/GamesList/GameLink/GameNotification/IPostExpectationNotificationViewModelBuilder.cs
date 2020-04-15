using DM.Services.Gaming.Dto.Output;

namespace DM.Web.Classic.Views.Shared.GamesList.GameLink.GameNotification
{
    public interface IPostExpectationNotificationViewModelBuilder
    {
        PostExpectationNotificationViewModel Build(Game game);

        PostExpectationNotificationViewModel Build(Room room);
    }
}