using System;
using DM.Services.Core.Implementation;
using DM.Services.Gaming.Dto.Output;
using DM.Web.Classic.Views.Shared.GamesList.GameLink.GameNotification;
using DM.Web.Classic.Views.Shared.User;
using Microsoft.EntityFrameworkCore.Internal;

namespace DM.Web.Classic.Views.Shared.GamesList.GameLink
{
    public class GameLinkViewModelBuilder : IGameLinkViewModelBuilder
    {
        private readonly IUserViewModelBuilder userViewModelBuilder;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IPostExpectationNotificationViewModelBuilder postExpectationNotificationViewModelBuilder;

        public GameLinkViewModelBuilder(
            IUserViewModelBuilder userViewModelBuilder,
            IDateTimeProvider dateTimeProvider,
            IPostExpectationNotificationViewModelBuilder postExpectationNotificationViewModelBuilder)
        {
            this.userViewModelBuilder = userViewModelBuilder;
            this.dateTimeProvider = dateTimeProvider;
            this.postExpectationNotificationViewModelBuilder = postExpectationNotificationViewModelBuilder;
        }

        public GameLinkViewModel Build(Game game, Guid? exceptGameId)
        {
            return new GameLinkViewModel
            {
                GameId = game.Id,
                Title = game.Title,
                Master = userViewModelBuilder.Build(game.Master),
                SystemName = game.SystemName,
                SettingName = game.SettingName,
                Status = game.Status,

                IsNew = game.ReleaseDate.HasValue && (dateTimeProvider.Now - game.ReleaseDate.Value).TotalDays < 2,

                HasPostNotification = game.Pendings.Any(),
                Notification = postExpectationNotificationViewModelBuilder.Build(game),

                HasUnreadCounters = true,
                UnreadPostsCount = game.UnreadPostsCount,
                UnreadCommentsCount = game.UnreadCommentsCount,
                UnreadCharactersCount = game.UnreadCharactersCount,

                IsCurrentGame = exceptGameId == game.Id
            };
        }
    }
}