using System;
using System.Collections.Generic;
using System.Linq;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Games;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.DataAccess.BusinessObjects.Games.Posts;
using DM.Services.Gaming.Dto.Input;

namespace DM.Services.Gaming.BusinessProcesses.Games.Creating
{
    /// <inheritdoc />
    public class GameFactory : IGameFactory
    {
        private readonly IGuidFactory guidFactory;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <inheritdoc />
        public GameFactory(
            IGuidFactory guidFactory,
            IDateTimeProvider dateTimeProvider)
        {
            this.guidFactory = guidFactory;
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <inheritdoc />
        public (Game game, Room room, IEnumerable<GameTag> tags) Create(CreateGame createGame,
            Guid userId, Guid? assistantId, GameStatus initialStatus)
        {
            var gameId = guidFactory.Create();
            var game = new Game
            {
                GameId = gameId,
                CreateDate = dateTimeProvider.Now,
                Status = initialStatus,
                MasterId = userId,
                AssistantId = assistantId,
                Title = createGame.Title,
                SystemName = createGame.SystemName,
                SettingName = createGame.SettingName,
                Info = createGame.Info,
                DisableAlignment = createGame.DisableAlignment,
                HideTemper = createGame.HideTemper,
                HideStory = createGame.HideStory,
                HideSkills = createGame.HideSkills,
                HideInventory = createGame.HideInventory,
                HideDiceResult = createGame.HideDiceResult,
                ShowPrivateMessages = createGame.ShowPrivateMessages,
                CommentariesAccessMode = createGame.CommentariesAccessMode
            };

            var room = new Room
            {
                RoomId = guidFactory.Create(),
                GameId = gameId,
                Type = RoomType.Default,
                AccessType = RoomAccessType.Open,
                Title = "Основная комната"
            };

            // TODO: game tags
            return (game, room, Enumerable.Empty<GameTag>());
        }
    }
}