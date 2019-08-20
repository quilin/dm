using System;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Games;
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
        public Game Create(CreateGame createGame,
            Guid userId, Guid? assistantId, GameStatus initialStatus)
        {
            return new Game
            {
                GameId = guidFactory.Create(),
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
        }
    }
}