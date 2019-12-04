using System;
using DM.Services.Core.Implementation;
using DM.Services.Gaming.Dto.Output;

namespace DM.Web.Classic.Views.Shared.GameList.GameLink
{
    public class GameLinkViewModelBuilder : IGameLinkViewModelBuilder
    {
        private readonly IDateTimeProvider dateTimeProvider;

        public GameLinkViewModelBuilder(
            IDateTimeProvider dateTimeProvider)
        {
            this.dateTimeProvider = dateTimeProvider;
        }

        public GameLinkViewModel Build(Game game, Guid? exceptGameId)
        {
            return new GameLinkViewModel
            {
                GameId = game.Id,
                Title = game.Title,
                MasterLogin = game.Master.Login,
                SystemName = game.SystemName,
                SettingName = game.SettingName,
                Status = game.Status,

                IsNew = game.ReleaseDate.HasValue && (dateTimeProvider.Now - game.ReleaseDate.Value).TotalDays < 2,

                HasUnreadCounters = true,
                UnreadCountPosts = game.UnreadPostsCount,
                UnreadCountComments = game.UnreadCommentsCount,
                UnseenCountCharacters = game.UnreadCharactersCount,

                IsCurrentModule = exceptGameId == game.Id
            };
        }

        public GameLinkViewModel BuildWithoutCounters(Game game)
        {
            return new GameLinkViewModel
            {
                GameId = game.Id,
                Title = game.Title,
                MasterLogin = game.Master.Login,
                SystemName = game.SystemName,
                SettingName = game.SettingName,
                Status = game.Status,

                IsNew = game.ReleaseDate.HasValue && (dateTimeProvider.Now - game.ReleaseDate.Value).TotalDays < 2,

                HasUnreadCounters = false,
                HasUnseenCharacterCounters = false,

                IsCurrentModule = false
            };
        }
    }
}