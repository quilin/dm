using System;
using DM.Services.Gaming.Dto.Output;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.GamesList.GamesListItem
{
    public class GamesListItemViewModelBuilder : IGamesListItemViewModelBuilder
    {
        private readonly IUserViewModelBuilder userViewModelBuilder;

        public GamesListItemViewModelBuilder(
            IUserViewModelBuilder userViewModelBuilder)
        {
            this.userViewModelBuilder = userViewModelBuilder;
        }

        public GamesListItemViewModel Build(Game game, int number, Guid? tagId)
        {
            return new GamesListItemViewModel
            {
                Number = number,
                GameId = game.Id,
                Title = game.Title,
                SystemName = game.SystemName,
                SettingName = game.SettingName,
                Status = game.Status,
                Master = userViewModelBuilder.Build(game.Master),

                UnreadCountPosts = game.UnreadPostsCount,
                UnreadCountComments = game.UnreadCommentsCount,

                Tags = game.Tags,
                SearchedTagId = tagId
            };
        }
    }
}